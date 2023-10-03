using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UsuariosController : BaseApiController
    {
        private readonly IUsuarioService _userService;

        public UsuariosController(IUsuarioService userService)
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
            SetRefreshTokenInCookie(result.RefreshToken);
            return Ok(result);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userService.RefreshTokenAsync(refreshToken);
            if(!string.IsNullOrEmpty(response.RefreshToken))
                SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(response);
        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10)
            };
            Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);

        }
    }
}
