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
        public async Task<ActionResult> NuevoUsuarioAsync(UsuarioNuevoDto model)
        {
            var result = await _userService.NuevoUsuarioAsync(model);
            return Ok(result);
        }

        [HttpPost("Token")]
        public async Task<IActionResult> GetTokenAsync(UsuarioLoginDto model)
        {
            var result = await _userService.GetTokenAsync(model);
            return Ok(result);
        }



    }
}
