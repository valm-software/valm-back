using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class AuthRolPermisoRepository : IAuthRolPermisoRepository
    {
        private readonly ValmContext _valmContext;

        public AuthRolPermisoRepository(ValmContext context)
        {
            _valmContext = context;
        }

        /// <summary>
        /// Añade una nueva relación Rol-Permiso.
        /// </summary>
        public async Task AddAsync(AuthRolPermiso authRolPermiso)
        {
            await _valmContext.AuthRolesPermisos.AddAsync(authRolPermiso);
            await _valmContext.SaveChangesAsync();
        }


        /// <summary>
        /// Actualiza una relación Rol-Permiso existente.
        /// </summary>
        /// <param name="rolIdAntiguo">El ID del rol antiguo.</param>
        /// <param name="permisoIdAntiguo">El ID del permiso antiguo.</param>
        /// <param name="rolIdNuevo">El ID del rol nuevo.</param>
        /// <param name="permisoIdNuevo">El ID del permiso nuevo.</param>
        public async Task UpdateAsync(int rolIdAntiguo, int permisoIdAntiguo, int rolIdNuevo, int permisoIdNuevo)
        {
            var authRolPermiso = await _valmContext.AuthRolesPermisos
                .Where(rp => rp.RolId == rolIdAntiguo && rp.PermisoId == permisoIdAntiguo)
                .FirstOrDefaultAsync();

            if (authRolPermiso != null)
            {
                authRolPermiso.RolId = rolIdNuevo;
                authRolPermiso.PermisoId = permisoIdNuevo;
                await _valmContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Elimina una relación Rol-Permiso.
        /// </summary>
        public async Task DeleteAsync(AuthRolPermiso authRolPermiso)
        {
            _valmContext.AuthRolesPermisos.Remove(authRolPermiso);
            await _valmContext.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene una lista de permisos asociados a un rol específico.
        /// </summary>
        /// <param name="rolId">El ID del rol.</param>
        /// <returns>Una lista de permisos asociados al rol.</returns>
        public async Task<IEnumerable<AuthPermiso>> GetPermisosByRolId(int rolId)
        {
            return await _valmContext.AuthRolesPermisos
                .Include(rp => rp.AuthPermiso)
                .Where(rp => rp.RolId == rolId)
                .Select(rp => rp.AuthPermiso)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una lista de roles asociados a un permiso específico.
        /// </summary>
        /// <param name="permisoId">El ID del permiso.</param>
        /// <returns>Una lista de roles asociados al permiso.</returns>
        public async Task<IEnumerable<AuthRol>> GetRolesByPermisoIdAsync(int permisoId)
        {
            return await _valmContext.AuthRolesPermisos
                .Include(rp => rp.AuthRol)
                .Where(rp => rp.PermisoId == permisoId)
                .Select(rp => rp.AuthRol)
                .ToListAsync();
        }

    }
}
