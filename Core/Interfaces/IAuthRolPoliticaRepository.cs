using Core.Entities;

public interface IAuthRolPoliticaRepository
{
    // Métodos específicos para gestionar la relación Rol-Permiso
    Task<IEnumerable<AuthRol>> GetRolesByPoliticaIdAsync(int permisoId);
}
