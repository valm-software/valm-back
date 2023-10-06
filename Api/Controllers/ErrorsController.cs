using Api.Helpers.Errores;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("errors/{code}")]
    public class ErrorsController  : BaseApiController
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }

    }
}
