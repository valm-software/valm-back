using Core.Entities;

public interface IAuthRolPermisoRepository
{
    // Métodos específicos para gestionar la relación Rol-Permiso
    Task<IEnumerable<AuthRol>> GetRolesByPermisoIdAsync(int permisoId);
}
