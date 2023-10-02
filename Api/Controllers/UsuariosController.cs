using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UsuariosController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsuariosController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("NuevoUsuario")]
        public async Task<ActionResult> NuevoUsuarioAsync(NuevoUsuarioDto model)
        {
            var result = await _userService.NuevoUsuarioAsync(model);
            return Ok(result);
        }

    }
}
