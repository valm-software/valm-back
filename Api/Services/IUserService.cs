using Api.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Api.Services
{
    public interface IUserService
    {
        Task<string> NuevoUsuarioAsync(NuevoUsuarioDto model);
    }
}
