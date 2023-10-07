using Api.Extensions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides;
using Api.Helpers.Errores;
using AspNetCoreRateLimit;

/*COSAS PARA CORREGIR
 * 
 * 1; Cuando las peticiones a la API superan el limite por IP y tiempo no retorna el error utilizando el ApiResponse
 * 2; Cuando se hace una peticion API en un formato diferente a json o xml  no retorna el error utilizando el ApiResponse
 * 3; Problema con el Accep en el encabezado de swagger cuado se corroja descomentar la linea  //options.ReturnHttpNotAcceptable = true;
 */

var builder = WebApplication.CreateBuilder(args);

// Configuración de Serilog para el logging
var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .CreateLogger();

// Añadir Serilog al sistema de logging
builder.Logging.AddSerilog(logger);

// Configuración de servicios y dependencias
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());
builder.Services.ConfigureCors();
builder.Services.AddAplicacionServices();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();

// Limitación de tasa
builder.Services.ConfigureRateLimitiong(builder.Configuration); 


builder.Services.AddControllers( options =>
{
    options.RespectBrowserAcceptHeader = true;
    //options.ReturnHttpNotAcceptable = true;

}).AddXmlSerializerFormatters();

builder.Services.AddValidationErrors();

// Configuración de autorización
builder.Services.AddAuthorization(options =>
{
    PolicyConfig.AddPolicies(options);
});

// Configuración de DbContext
builder.Services.AddDbContext<ValmContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MariaDBConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Configuración adicional para producción
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        // Para el puerto 5000
        serverOptions.ListenAnyIP(5000);

        // Para SSL en el puerto 5002
        serverOptions.Listen(System.Net.IPAddress.Any, 5002, listenOptions =>
        {
            listenOptions.UseHttps("/etc/nginx/ssl/certValm2023.pfx", "valm2023");
        });
    });
}

var app = builder.Build();

// Middleware para manejo de excepciones globales
app.UseMiddleware<ExceptionMiddleware>();

// Middleware para manejar errores de estado
app.UseStatusCodePagesWithReExecute("/errors/{0}");

// Middleware para manejar cabeceras forwarded (importantes para obtener la IP real en una configuración de proxy inverso)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Middleware de AspNetCoreRateLimit para limitación de tasa (debe ir después de cualquier middleware que pueda cambiar la IP)
app.UseIpRateLimiting();

// Middleware personalizado para manejar la respuesta de límite de tasa
//app.UseMiddleware<CustomIpRateLimitMiddleware>();


// Middleware personalizado para logging con Serilog
app.UseMiddleware<SerilogIpEnricherMiddleware>();

// Middleware para Swagger (documentación API)
app.UseSwagger();
app.UseSwaggerUI();

// Migraciones y seed de la base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerfactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<ValmContext>();
        await context.Database.MigrateAsync();
        await ValmContextSeed.SeedAsync(context, loggerfactory);
    }
    catch (Exception ex)
    {
        var logger2 = loggerfactory.CreateLogger<Program>();
        logger2.LogError(ex, "Ocurrió un error durante la migración");
    }
}

// Configuración de CORS
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Middleware para redirección HTTPS
app.UseHttpsRedirection();

// Middleware para autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Middleware personalizado para logging con Serilog
app.UseMiddleware<SerilogUserNameEnricherMiddleware>();

// Configuración de endpoints
app.MapControllers();

// Iniciar la aplicación
app.Run();
