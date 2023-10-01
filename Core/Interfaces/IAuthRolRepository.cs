using Core.Entities;

namespace Core.Interfaces
{
    public interface IAuthRolRepository : IGenericRepository<AuthRol>
    {
        // Métodos específicos para gestionar roles, como obtener todos los permisos de un rol
        Task<IEnumerable<AuthPermiso>> GetPermisosByRolIdAsync(int rolId);
    }
}
