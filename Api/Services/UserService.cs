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

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly JWT _jwt;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<AuthUsuario> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<AuthUsuario> passwordHasher)
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
                Dni = nuevoUsuarioDto.Dni,
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
                usuarioDatosDto.EstadoAtenticado = false;
                usuarioDatosDto.Mensaje = $"No existe ningún nombre con el usuario {model.Usuario}.";
                return usuarioDatosDto;
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

            if (resultado == PasswordVerificationResult.Success)
            {
                usuarioDatosDto.EstadoAtenticado = true;
                JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
                usuarioDatosDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                usuarioDatosDto.Correo = usuario.Correo;
                usuarioDatosDto.Nombre = usuario.Nombre;
                usuarioDatosDto.Usuario = usuario.Usuario;

                // Mapeo de roles y permisos
                usuarioDatosDto.Roles = usuario.AuthUsuarioRoles.Select(ur => new RolConPermisosDto
                {
                    Rol = ur.AuthRol.Nombre,
                    Modulos = ur.AuthRol.AuthRolPermisos
                               .GroupBy(rp => rp.AuthPermiso.Modulo)
                               .Select(g => new ModuloConPermisosDto
                               {
                                   Modulo = g.Key,
                                   Permisos = g.Select(rp => rp.AuthPermiso.Nombre).ToList()
                               }).ToList()
                }).ToList();

                return usuarioDatosDto;
            }

            usuarioDatosDto.EstadoAtenticado = false;
            usuarioDatosDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.Usuario}.";
            return usuarioDatosDto;
        }

        private JwtSecurityToken CreateJwtToken(AuthUsuario usuario)
        {
            var roles = usuario.AuthUsuarioRoles.Select(ur => ur.AuthRol).ToList();
            var roleClaims = new List<Claim>();
            var policyClaims = new List<Claim>();  // Nueva lista de claims para políticas

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role.Nombre));
                foreach (var permiso in role.AuthRolPermisos)
                {
                    policyClaims.Add(new Claim("policy", permiso.AuthPermiso.Nombre));  // Añadiendo cada permiso como una política
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


        //private JwtSecurityToken CreateJwtToken(AuthUsuario usuario)
        //    {
        //        var rolesList = usuario.AuthUsuarioRoles.Select(ur => new
        //        {
        //            Rol = ur.AuthRol.Nombre,
        //            Modulos = ur.AuthRol.AuthRolPermisos
        //                       .GroupBy(rp => rp.AuthPermiso.Modulo)
        //                       .Select(g => new
        //                       {
        //                           Modulo = g.Key,
        //                           Permisos = g.Select(rp => rp.AuthPermiso.Nombre).ToList()
        //                       }).ToList()
        //        }).ToList();

        //        // Serializar rolesList a JSON
        //        var rolesJson = JsonConvert.SerializeObject(rolesList);

        //        var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
        //        new Claim("uid", usuario.Id.ToString()),
        //        new Claim("roles", rolesJson) // Añadir roles como JSON
        //    };

        //        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        //        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        //        var jwtSecurityToken = new JwtSecurityToken(
        //            issuer: _jwt.Issuer,
        //            audience: _jwt.Audience,
        //            claims: claims,
        //            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
        //            signingCredentials: signingCredentials
        //        );

        //        return jwtSecurityToken;
        //    }


        //private JwtSecurityToken CreateJwtToken(AuthUsuario usuario)
        //{
        //    var roles = usuario.AuthUsuarioRoles.Select(ur => ur.AuthRol).ToList();
        //    var roleClaims = new List<Claim>();
        //    var permisosClaims = new List<Claim>();

        //    foreach (var role in roles)
        //    {
        //        roleClaims.Add(new Claim("roles", role.Nombre));
        //        foreach (var permiso in role.AuthRolPermisos)
        //        {
        //            // Aquí asumo que tienes una propiedad `Nombre` en el objeto permiso.
        //            permisosClaims.Add(new Claim("permisos", permiso.AuthPermiso.Nombre));
        //        }
        //    }

        //    var claims = new[]
        //    {
        //    new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
        //    new Claim("uid", usuario.Id.ToString())
        //}
        //    .Union(roleClaims)
        //    .Union(permisosClaims);  // Añadimos los claims de permisos aquí

        //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        //    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        //    var jwtSecurityToken = new JwtSecurityToken(
        //        issuer: _jwt.Issuer,
        //        audience: _jwt.Audience,
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
        //        signingCredentials: signingCredentials
        //    );

        //    return jwtSecurityToken;
        //}


        //private JwtSecurityToken CreateJwtToken(AuthUsuario usuario)
        //{
        //    var roles = usuario.AuthUsuarioRoles.Select(ur => ur.AuthRol).ToList();
        //    var roleClaims = new List<Claim>();

        //    foreach (var role in roles)
        //    {
        //        roleClaims.Add(new Claim("roles", role.Nombre));
        //    }

        //    // Aquí también puedes añadir claims para permisos y módulos si los necesitas

        //    var claims = new[]
        //    {
        //            new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
        //            new Claim("uid", usuario.Id.ToString())
        //    }
        //    .Union(roleClaims);

        //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        //    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        //    var jwtSecurityToken = new JwtSecurityToken(
        //        issuer: _jwt.Issuer,
        //        audience: _jwt.Audience,
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
        //        signingCredentials: signingCredentials
        //    );

        //    return jwtSecurityToken;
        //}

    }
}
