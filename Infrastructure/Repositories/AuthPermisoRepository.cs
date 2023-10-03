using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class AuthPermisoRepository : GenericRepository<AuthPolitica>, IAuthPoliticaRepository
    {
        public AuthPermisoRepository(ValmContext context) : base(context)
        {

        }

        /// <summary>
        /// Obtiene una lista de permisos por módulo.
        /// </summary>
        /// <param name="modulo">El nombre del módulo.</param>
        /// <returns>Una lista de permisos.</returns>
        public async Task<IEnumerable<AuthPolitica>> GetPermisosByModuloAsync(string modulo)
        {
            return await _valmContext.AuthPoliticas
                .Where(p => p.Modulo == modulo)
                .ToListAsync();
        }

        /// <summary>
        /// Busca permisos por un término de búsqueda en el nombre.
        /// </summary>
        /// <param name="searchTerm">El término de búsqueda.</param>
        /// <returns>Una lista de permisos que coinciden con el término de búsqueda.</returns>
        public async Task<IEnumerable<AuthPolitica>> SearchPermisosAsync(string searchTerm)
        {
            return await _valmContext.AuthPoliticas
                .Where(p => p.NombrePolitica.Contains(searchTerm))
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una lista de usuarios asociados a un permiso específico.
        /// </summary>
        /// <param name="permisoId">El ID del permiso.</param>
        /// <returns>Una lista de usuarios asociados al permiso.</returns>
        public async Task<IEnumerable<AuthUsuario>> GetUsuariosByPermisoIdAsync(int permisoId)
        {
            // Obtener los roles que tienen asignado este permiso específico.
            // Estamos seleccionando solamente los IDs de los roles para usarlos más adelante.
            var rolesConPermiso = await _valmContext.AuthRolesPoliticas
                .Where(rp => rp.PoliticaId == permisoId)
                .Select(rp => rp.RolId)
                .ToListAsync();


            // Ahora, vamos a buscar los usuarios que están asociados a esos roles.
            // Aquí es donde entra la tabla AuthUsuarioRoles.
            // Utilizamos el método "Where" para filtrar solamente los registros de AuthUsuarioRoles que tengan un RolId presente en nuestra lista "rolesConPermiso".
            var usuariosConRol = await _valmContext.AuthUsuariosRoles
                .Where(ur => rolesConPermiso.Contains(ur.RolId))
                .Include(ur => ur.AuthUsuario) // Aquí estamos incluyendo la entidad relacionada AuthUsuario para cada registro de AuthUsuarioRol.
                .Select(ur => ur.AuthUsuario) // Seleccionamos solamente la parte de AuthUsuario de cada registro de AuthUsuarioRol.
                .ToListAsync();

            return usuariosConRol;
        }

        /// <summary>
        /// Verifica si un permiso es válido (existe en la base de datos).
        /// </summary>
        /// <param name="permisoId">El ID del permiso.</param>
        /// <returns>Verdadero si el permiso es válido, falso en caso contrario.</returns>
        public async Task<bool> IsValidPermisoAsync(int permisoId)
        {
            return await _valmContext.AuthPoliticas
                .AnyAsync(p => p.Id == permisoId);
        }

        /// <summary>
        /// Cuenta el número total de permisos en la base de datos.
        /// </summary>
        /// <returns>El número total de permisos.</returns>
        public async Task<int> CountPermisosAsync()
        {
            return await _valmContext.AuthPoliticas.CountAsync();
        }

        /// <summary>
        /// Cuenta el número de permisos asociados a un módulo específico.
        /// </summary>
        /// <param name="modulo">El nombre del módulo.</param>
        /// <returns>El número de permisos asociados al módulo.</returns>
        public async Task<int> CountPermisosByModuloAsync(string modulo)
        {
            return await _valmContext.AuthPoliticas
                .Where(p => p.Modulo == modulo)
                .CountAsync();
        }
    }
}
