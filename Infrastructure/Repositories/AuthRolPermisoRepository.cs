using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class AuthRolPermisoRepository : IAuthRolPoliticaRepository
    {
        private readonly ValmContext _valmContext;

        public AuthRolPermisoRepository(ValmContext context)
        {
            _valmContext = context;
        }

        /// <summary>
        /// Añade una nueva relación Rol-Permiso.
        /// </summary>
        public async Task AddAsync(AuthRolPolitica authRolPermiso)
        {
            await _valmContext.AuthRolesPoliticas.AddAsync(authRolPermiso);
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
            var authRolPermiso = await _valmContext.AuthRolesPoliticas
                .Where(rp => rp.RolId == rolIdAntiguo && rp.PoliticaId == permisoIdAntiguo)
                .FirstOrDefaultAsync();

            if (authRolPermiso != null)
            {
                authRolPermiso.RolId = rolIdNuevo;
                authRolPermiso.PoliticaId = permisoIdNuevo;
                await _valmContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Elimina una relación Rol-Permiso.
        /// </summary>
        public async Task DeleteAsync(AuthRolPolitica authRolPermiso)
        {
            _valmContext.AuthRolesPoliticas.Remove(authRolPermiso);
            await _valmContext.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene una lista de permisos asociados a un rol específico.
        /// </summary>
        /// <param name="rolId">El ID del rol.</param>
        /// <returns>Una lista de permisos asociados al rol.</returns>
        public async Task<IEnumerable<AuthPolitica>> GetPermisosByRolId(int rolId)
        {
            return await _valmContext.AuthRolesPoliticas
                .Include(rp => rp.AuthPolitica)
                .Where(rp => rp.RolId == rolId)
                .Select(rp => rp.AuthPolitica)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una lista de roles asociados a un permiso específico.
        /// </summary>
        /// <param name="permisoId">El ID del permiso.</param>
        /// <returns>Una lista de roles asociados al permiso.</returns>
        public async Task<IEnumerable<AuthRol>> GetRolesByPoliticaIdAsync(int permisoId)
        {
            return await _valmContext.AuthRolesPoliticas
                .Include(rp => rp.AuthRol)
                .Where(rp => rp.PoliticaId == permisoId)
                .Select(rp => rp.AuthRol)
                .ToListAsync();
        }

    }
}
