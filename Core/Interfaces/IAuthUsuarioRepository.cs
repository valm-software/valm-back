using Core.Entities;

namespace Core.Interfaces
{
    public interface IAuthUsuarioRepository : IGenericRepository<AuthUsuario>
    {
        // Métodos específicos para gestionar usuarios, como autenticación, cambio de contraseña, etc.
    }
}
