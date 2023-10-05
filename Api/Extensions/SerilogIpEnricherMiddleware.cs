using Serilog.Context;

public class SerilogIpEnricherMiddleware
{
    private readonly RequestDelegate _next;

    public SerilogIpEnricherMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var remoteIpAddress = context.Connection.RemoteIpAddress?.ToString();

        // Intenta obtener la dirección IP del cliente desde la cabecera X-Forwarded-For
        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            remoteIpAddress = context.Request.Headers["X-Forwarded-For"];
        }

        if (remoteIpAddress != null)
        {
            using (LogContext.PushProperty("ClientIP", remoteIpAddress))
            {
                await _next(context);
            }
        }
        else
        {
            await _next(context);
        }
    }

}
