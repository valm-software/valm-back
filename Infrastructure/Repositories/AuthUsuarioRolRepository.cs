using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuthUsuarioRolRepository : IAuthUsuarioRolRepository
    {
        private readonly ValmContext _valmContext;

        public AuthUsuarioRolRepository(ValmContext valmContext)
        {
            _valmContext = valmContext;
        }

        /// <summary>
        /// Asigna un rol a un usuario.
        /// </summary>
        /// <param name="usuarioId">El ID del usuario.</param>
        /// <param name="rolId">El ID del rol.</param>
        public async Task AsignarRolAUsuario(int usuarioId, int rolId)
        {
            var usuarioRol = new AuthUsuarioRol
            {
                UsuarioId = usuarioId,
                RolId = rolId
            };

            _valmContext.AuthUsuariosRoles.Add(usuarioRol);
            await _valmContext.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza el rol de un usuario.
        /// </summary>
        /// <param name="usuarioId">El ID del usuario.</param>
        /// <param name="rolIdAntiguo">El ID del rol antiguo.</param>
        /// <param name="rolIdNuevo">El ID del rol nuevo.</param>
        public async Task ActualizarRolDeUsuario(int usuarioId, int rolIdAntiguo, int rolIdNuevo)
        {
            var usuarioRol = await _valmContext.AuthUsuariosRoles
                .Where(ur => ur.UsuarioId == usuarioId && ur.RolId == rolIdAntiguo)
                .FirstOrDefaultAsync();

            if (usuarioRol != null)
            {
                usuarioRol.RolId = rolIdNuevo;
                await _valmContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Elimina un rol de un usuario.
        /// </summary>
        /// <param name="usuarioId">El ID del usuario.</param>
        /// <param name="rolId">El ID del rol.</param>
        public async Task EliminarRolDeUsuario(int usuarioId, int rolId)
        {
            var usuarioRol = await _valmContext.AuthUsuariosRoles
                .Where(ur => ur.UsuarioId == usuarioId && ur.RolId == rolId)
                .FirstOrDefaultAsync();

            if (usuarioRol != null)
            {
                _valmContext.AuthUsuariosRoles.Remove(usuarioRol);
                await _valmContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Obtiene una lista de roles asociados a un usuario específico.
        /// </summary>
        /// <param name="usuarioId">El ID del usuario.</param>
        /// <returns>Una lista de roles asociados al usuario.</returns>
        public async Task<IEnumerable<AuthRol>> GetRolesByUsuarioId(int usuarioId)
        {
            return await _valmContext.AuthUsuariosRoles
                .Where(ur => ur.UsuarioId == usuarioId)
                .Select(ur => ur.AuthRol)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una lista de usuarios asociados a un rol específico.
        /// </summary>
        /// <param name="rolId">El ID del rol.</param>
        /// <returns>Una lista de usuarios asociados al rol.</returns>
        public async Task<IEnumerable<AuthUsuario>> GetUsuariosByRolId(int rolId)
        {
            return await _valmContext.AuthUsuariosRoles
                .Where(ur => ur.RolId == rolId)
                .Select(ur => ur.AuthUsuario)
                .ToListAsync();
        }
    }
}
