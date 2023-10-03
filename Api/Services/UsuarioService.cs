using Api.Helpers;
using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Api.Dtos;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json; // Asegúrate de importar este espacio de nombres para JsonConvert
using System.Security.Cryptography;

namespace Api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly JWT _jwt;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<AuthUsuario> _passwordHasher;

        public UsuarioService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<AuthUsuario> passwordHasher)
        {
            _jwt = jwt.Value;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;

        }

        public async Task<string> NuevoUsuarioAsync(UsuarioNuevoDto nuevoUsuarioDto)
        {
            // Crear el nuevo usuario
            var usuario = new AuthUsuario
            {
                DNI = nuevoUsuarioDto.DNI,
                Nombre = nuevoUsuarioDto.Nombre,
                Correo = nuevoUsuarioDto.Correo,
                Usuario = nuevoUsuarioDto.Usuario,
                AuthUsuarioRoles = new List<AuthUsuarioRol>()  // Inicializar la lista
            };

            usuario.Password = _passwordHasher.HashPassword(usuario, nuevoUsuarioDto.Password);

            // Verificar si el usuario ya existe
            var usuarioExiste = _unitOfWork.AuthUsuarios
                                           .Find(u => u.Usuario.ToLower() == nuevoUsuarioDto.Usuario.ToLower())
                                           .FirstOrDefault();

            // Si el usuario no existe, proceder a crearlo
            if (usuarioExiste == null)
            {
                // Iniciar la transacción
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    // Obtener el rol por defecto (asumiendo que el ID del rol por defecto es 9)
                    var rolPredeterminado = await _unitOfWork.AuthRoles.GetByIdAsync(9);
                    if (rolPredeterminado != null)
                    {
                        // Crear la nueva relación Usuario-Rol
                        var usuarioRol = new AuthUsuarioRol
                        {
                            RolId = rolPredeterminado.Id
                        };

                        // Añadir a la colección de roles del usuario
                        usuario.AuthUsuarioRoles.Add(usuarioRol);

                        // Agregar el nuevo usuario
                        _unitOfWork.AuthUsuarios.Add(usuario);

                        // Guardar los cambios y confirmar la transacción
                        await _unitOfWork.SaveAsync();
                        await _unitOfWork.CommitTransactionAsync();

                        return $"El Usuario  {nuevoUsuarioDto.Usuario} se ha creado con éxito y el rol asignado es: {rolPredeterminado.Id} ";
                    }
                    else
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return "El rol predeterminado no existe.";
                    }
                }
                catch (Exception ex)
                {
                    // Hacer un rollback en caso de una excepción
                    await _unitOfWork.RollbackTransactionAsync();
                    return $"Se produjo un error al crear el usuario: {ex.Message}";
                }
            }
            else
            {
                return "El usuario ya existe.";
            }
        }

        public async Task<UsuarioDatosDto> GetTokenAsync(UsuarioLoginDto model)
        {
            UsuarioDatosDto usuarioDatosDto = new UsuarioDatosDto();
            var usuario = await _unitOfWork.AuthUsuarios.GetByUsuarioAsync(model.Usuario);

            if (usuario == null)
            {
                usuarioDatosDto.EstadoAutenticado = false;
                usuarioDatosDto.Mensaje = $"No existe ningún nombre con el usuario {model.Usuario}.";
                return usuarioDatosDto;
            }

            try
            {
                var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

                if (resultado == PasswordVerificationResult.Success)
                {
                    usuarioDatosDto.EstadoAutenticado = true;
                    JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
                    usuarioDatosDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    usuarioDatosDto.Correo = usuario.Correo;
                    usuarioDatosDto.Nombre = usuario.Nombre;
                    usuarioDatosDto.Usuario = usuario.Usuario;

                    // Mapeo de roles y permisos
                    usuarioDatosDto.Roles = usuario.AuthUsuarioRoles.Select(ur => new RolConPoliticasDto
                    {
                        Rol = ur.AuthRol.NombreRol,
                        Modulos = ur.AuthRol.AuthRolPoliticas
                                   .GroupBy(rp => rp.AuthPolitica.Modulo)
                                   .Select(g => new ModuloConPoliticasDto
                                   {
                                       Modulo = g.Key,
                                       Politicas = g.Select(rp => rp.AuthPolitica.NombrePolitica).ToList()
                                   }).ToList()
                    }).ToList();

                    if(usuario.AuthRefreshToken.Any(a => a.EstaActivo))
                    {
                        var ActiveRefreshToket = usuario.AuthRefreshToken.Where(a => a.EstaActivo == true).FirstOrDefault();
                        usuarioDatosDto.RefreshToken = ActiveRefreshToket.Token;
                        usuarioDatosDto.RefreshTokenExpirado = ActiveRefreshToket.Expirado;
                    }
                    else
                    {
                        var refreshToken = CreateRefreshToken();
                        usuarioDatosDto.RefreshToken = refreshToken.Token;
                        usuarioDatosDto.RefreshTokenExpirado = refreshToken.Expirado;
                        usuario.AuthRefreshToken.Add(refreshToken);
                        _unitOfWork.AuthUsuarios.Update(usuario);
                        await _unitOfWork.SaveAsync();
                    }
                    return usuarioDatosDto;
                }
                else
                {
                    usuarioDatosDto.EstadoAutenticado = false;
                    usuarioDatosDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.Usuario}.";
                    return usuarioDatosDto;
                }
            }
            catch (FormatException ex)
            {
                usuarioDatosDto.EstadoAutenticado = false;
                usuarioDatosDto.Mensaje = "La contraseña no está codificada correctamente. Detalles: " + ex.Message;
                return usuarioDatosDto;
            }
        }

        private JwtSecurityToken CreateJwtToken(AuthUsuario usuario)
        {
            var roles = usuario.AuthUsuarioRoles.Select(ur => ur.AuthRol).ToList();
            var roleClaims = new List<Claim>();
            var policyClaims = new List<Claim>();  // Nueva lista de claims para políticas

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role.NombreRol));
                foreach (var permiso in role.AuthRolPoliticas)
                {
                    policyClaims.Add(new Claim("policy", permiso.AuthPolitica.NombrePolitica));  // Añadiendo cada permiso como una política
                }
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
                new Claim("uid", usuario.Id.ToString())
            }
            .Union(roleClaims)
            .Union(policyClaims);  // Añadimos los claims de políticas aquí

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }

        public async Task<UsuarioDatosDto> RefreshTokenAsync(string refreshToken)
        {
            var usuarioDatosDto = new UsuarioDatosDto();

            var usuario = await _unitOfWork.AuthUsuarios
                            .GetByResfrestokenAsync(refreshToken);

            if (usuarioDatosDto == null)
            {
                usuarioDatosDto.EstadoAutenticado = false;
                usuarioDatosDto.Mensaje = $"El token no pertenece a ningun usuario.";
                return usuarioDatosDto;
            }
            var refreshTokenBd = usuario.AuthRefreshToken.Single(x => x.Token == refreshToken);

            if (!refreshTokenBd.EstaActivo)
            {
                usuarioDatosDto.EstadoAutenticado = false;
                usuarioDatosDto.Mensaje = $"El token no esta activo. ";
                return usuarioDatosDto;
            }

            //revocamos el refresh token actual 
            refreshTokenBd.Revocado = DateTime.UtcNow;
            
            // generamos un nuevo refresh Token y lo guardamos en la base de datos 
            var newRefreshToken = CreateRefreshToken();
            usuario.AuthRefreshToken.Add(newRefreshToken);
            _unitOfWork.AuthUsuarios.Update(usuario);
            await _unitOfWork.SaveAsync();

            //Generamos un nuevo Json web token 
            usuarioDatosDto.EstadoAutenticado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
            usuarioDatosDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            usuarioDatosDto.Correo = usuario.Correo;
            usuarioDatosDto.Nombre = usuario.Nombre;
            // Mapeo de roles y permisos
            usuarioDatosDto.Roles = usuario.AuthUsuarioRoles.Select(ur => new RolConPoliticasDto
            {
                Rol = ur.AuthRol.NombreRol,
                Modulos = ur.AuthRol.AuthRolPoliticas
                           .GroupBy(rp => rp.AuthPolitica.Modulo)
                           .Select(g => new ModuloConPoliticasDto
                           {
                               Modulo = g.Key,
                               Politicas = g.Select(rp => rp.AuthPolitica.NombrePolitica).ToList()
                           }).ToList()
            }).ToList();
            usuarioDatosDto.RefreshToken = newRefreshToken.Token;
            usuarioDatosDto.RefreshTokenExpirado = newRefreshToken.Expirado;
            return usuarioDatosDto;
        }


        private AuthRefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomNumber);
                return new AuthRefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expirado = DateTime.UtcNow.AddDays(10),
                    Creado = DateTime.UtcNow
                };
            }

        }

        /**
         * Alternativas a como obtener el token
         * 
         * 
         * 
        private JwtSecurityToken CreateJwtToken(AuthUsuario usuario)
        {
            var rolesList = usuario.AuthUsuarioRoles.Select(ur => new
            {
                Rol = ur.AuthRol.NombrePolitica,
                Modulos = ur.AuthRol.AuthRolPoliticas
                           .GroupBy(rp => rp.AuthPolitica.Modulo)
                           .Select(g => new
                           {
                               Modulo = g.Key,
                               Politicas = g.Select(rp => rp.AuthPolitica.NombrePolitica).ToList()
                           }).ToList()
            }).ToList();

            // Serializar rolesList a JSON
            var rolesJson = JsonConvert.SerializeObject(rolesList);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
                new Claim("uid", usuario.Id.ToString()),
                new Claim("roles", rolesJson) // Añadir roles como JSON
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }


        private JwtSecurityToken CreateJwtToken(AuthUsuario usuario)
        {
            var roles = usuario.AuthUsuarioRoles.Select(ur => ur.AuthRol).ToList();
            var roleClaims = new List<Claim>();
            var permisosClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role.NombrePolitica));
                foreach (var permiso in role.AuthRolPoliticas)
                {
                    // Aquí asumo que tienes una propiedad `NombrePolitica` en el objeto permiso.
                    permisosClaims.Add(new Claim("permisos", permiso.AuthPolitica.NombrePolitica));
                }
            }

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
            new Claim("uid", usuario.Id.ToString())
        }
            .Union(roleClaims)
            .Union(permisosClaims);  // Añadimos los claims de permisos aquí

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }


        private JwtSecurityToken CreateJwtToken(AuthUsuario usuario)
        {
            var roles = usuario.AuthUsuarioRoles.Select(ur => ur.AuthRol).ToList();
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role.NombrePolitica));
            }

            // Aquí también puedes añadir claims para permisos y módulos si los necesitas

            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
                    new Claim("uid", usuario.Id.ToString())
            }
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }
        */

    }
}
