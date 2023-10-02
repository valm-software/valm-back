using Core.Entities;

public interface IAuthUsuarioRolRepository
{
    // Aquí puedes agregar métodos específicos para gestionar la relación Usuario-Rol
    // Por ejemplo, si quieres obtener todos los roles de un usuario específico
    Task<IEnumerable<AuthRol>> GetRolesByUsuarioId(int usuarioId);
    Task AsignarRolAUsuario(int usuarioId, int rolId); 


    // Otros métodos que puedas necesitar
}