using Core.Entities;

namespace Core.Interfaces
{
    public interface IAuthUsuarioRepository : IGenericRepository<AuthUsuario>
    {        
        Task<AuthUsuario> GetByUsuarioAsync(string usuario);
        Task<AuthUsuario> GetByResfrestokenAsync(string refreshToken);


    }
}
