using Core.Interfaces;
using Serilog.Context;
using System.Security.Claims;

public class SerilogUserNameEnricherMiddleware
{
    private readonly RequestDelegate _next;

    public SerilogUserNameEnricherMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        string userName = "Anonymous";

        using (var scope = serviceProvider.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                // Asumiendo que el nombre de usuario se guarda en algún claim
                var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    var usuario = await unitOfWork.AuthUsuarios.GetByUsuarioAsync(userIdClaim.Value);
                    if (usuario != null)
                    {
                        userName = usuario.Usuario;
                    }
                }
            }
        }

        LogContext.PushProperty("UserName", userName);
        await _next(context);
    }
}
