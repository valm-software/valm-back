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
        if (remoteIpAddress != null)
        {
            LogContext.PushProperty("ClientIP", remoteIpAddress);
        }

        // Call the next delegate/middleware in the pipeline
        await _next(context);
    }
}
