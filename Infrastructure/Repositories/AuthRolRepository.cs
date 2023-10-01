using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuthRolRepository : GenericRepository<AuthRol>, IAuthRolRepository
    {
        public AuthRolRepository(ValmContext context) : base(context) { }

        /// <summary>
        /// Obtiene todos los permisos para un rol específico.
        /// </summary>
        /// <param name="rolId">El ID del rol.</param>
        /// <returns>Una lista de permisos asociados al rol.</returns>
        public async Task<IEnumerable<AuthPermiso>> GetPermisosByRolIdAsync(int rolId)
        {
            return await _valmContext.AuthRolesPermisos
                .Include(rp => rp.AuthPermiso)
                .Where(rp => rp.RolId == rolId)
                .Select(rp => rp.AuthPermiso)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene todos los usuarios para un rol específico.
        /// </summary>
        /// <param name="rolId">El ID del rol.</param>
        /// <returns>Una lista de usuarios asociados al rol.</returns>
        public async Task<IEnumerable<AuthUsuario>> GetUsuariosByRolIdAsync(int rolId)
        {
            return await _valmContext.AuthUsuariosRoles
                .Include(ur => ur.AuthUsuario)
                .Where(ur => ur.RolId == rolId)
                .Select(ur => ur.AuthUsuario)
                .ToListAsync();
        }

    }
}
