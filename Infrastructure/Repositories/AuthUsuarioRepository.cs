using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuthUsuarioRepository : GenericRepository<AuthUsuario>, IAuthUsuarioRepository
    {
        public AuthUsuarioRepository(ValmContext context) : base(context) { }

        /// <summary>
        /// Obtiene una lista de roles asociados a un usuario específico.
        /// </summary>
        /// <param name="usuarioId">El ID del usuario.</param>
        /// <returns>Una lista de roles asociados al usuario.</returns>
        public async Task<IEnumerable<AuthRol>> GetRolesByUsuarioId(int usuarioId)
        {
            return await _valmContext.AuthUsuariosRoles
                .Include(ur => ur.AuthRol)
                .Where(ur => ur.UsuarioId == usuarioId)
                .Select(ur => ur.AuthRol)
                .ToListAsync();
        }

        /// <summary>
        /// Verifica si un usuario tiene un rol específico.
        /// </summary>
        /// <param name="usuarioId">El ID del usuario.</param>
        /// <param name="rolNombre">El nombre del rol.</param>
        /// <returns>True si el usuario tiene el rol, de lo contrario False.</returns>
        public async Task<bool> UsuarioHasRol(int usuarioId, string rolNombre)
        {
            return await _valmContext.AuthUsuariosRoles
                .AnyAsync(ur => ur.UsuarioId == usuarioId && ur.AuthRol.NombreRol == rolNombre);
        }

        /// <summary>
        /// Verifica si un usuario tiene un permiso específico.
        /// </summary>
        /// <param name="usuarioId">El ID del usuario.</param>
        /// <param name="permisoNombre">El nombre del permiso.</param>
        /// <returns>True si el usuario tiene el permiso, de lo contrario False.</returns>
        public async Task<bool> UsuarioHasPermiso(int usuarioId, string permisoNombre)
        {
            var permisos = await GetPermisosByUsuarioId(usuarioId);
            return permisos.Any(p => p.NombrePolitica == permisoNombre);
        }


        /// <summary>
        /// Obtiene una lista de permisos asociados a un usuario específico.
        /// </summary>
        /// <param name="usuarioId">El ID del usuario.</param>
        /// <returns>Una lista de permisos asociados al usuario.</returns>
        public async Task<IEnumerable<AuthPolitica>> GetPermisosByUsuarioId(int usuarioId)
        {
            // Obtener los roles del usuario
            var rolesDelUsuario = await _valmContext.AuthUsuariosRoles
                .Where(ur => ur.UsuarioId == usuarioId)
                .Select(ur => ur.RolId)
                .ToListAsync();

            // Obtener los permisos de esos roles
            var permisos = await _valmContext.AuthRolesPoliticas
                .Where(rp => rolesDelUsuario.Contains(rp.RolId))
                .Select(rp => rp.AuthPolitica)
                .ToListAsync();

            return permisos;
        }



        /// <summary>
        /// Obtiene un usuario por su dirección de correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>El usuario que corresponde al correo electrónico.</returns>
        public async Task<AuthUsuario> GetUsuarioByEmail(string email)
        {
            return await _valmContext.AuthUsuarios
                .FirstOrDefaultAsync(u => u.Correo == email);
        }


        /// <summary>
        /// Obtiene lso datos del usuario por su nombre.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>El usuario que corresponde al correo electrónico.</returns>
        public async Task<AuthUsuario> GetByUsuarioAsync(string usuario)
        {
            return await _valmContext.AuthUsuarios
                .Include(u => u.AuthUsuarioRoles) // Incluye la relación AuthUsuarioRoles
                    .ThenInclude(ur => ur.AuthRol) // Desde AuthUsuarioRoles, incluye AuthRol
                        .ThenInclude(r => r.AuthRolPoliticas) // Desde AuthRol, incluye AuthRolPoliticas
                            .ThenInclude(rp => rp.AuthPolitica) // Desde AuthRolPoliticas, incluye AuthPolitica
                .FirstOrDefaultAsync(u => u.Usuario.ToLower() == usuario.ToLower());
        }



    }
}
