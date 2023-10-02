using Api.Helpers;
using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly JWT _jwt;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<AuthUsuario> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<AuthUsuario> passwordHasher)
        {
            _jwt =jwt.Value;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;

        }

        public async Task<string> NuevoUsuarioAsync(NuevoUsuarioDto nuevoUsuarioDto)
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



    }
}
