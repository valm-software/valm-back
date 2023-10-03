using Api.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Api.Services
{
    public interface IUsuarioService
    {
        Task<string> NuevoUsuarioAsync(UsuarioNuevoDto model);
        Task<UsuarioDatosDto> GetTokenAsync(UsuarioLoginDto model);

        Task<UsuarioDatosDto> RefreshTokenAsync(string refreshToken);
    }
}
