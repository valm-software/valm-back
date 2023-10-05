using Api.Extensions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());
builder.Services.ConfigureCors();
builder.Services.AddAplicacionServices();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();

builder.Services.AddControllers();

// Configura la autorización y añade las políticas
builder.Services.AddAuthorization(options =>
{
    // Aquí es donde añadimos las políticas
    PolicyConfig.AddPolicies(options);
});

// Este es actualizado y autodetecta la version de mariaDb
builder.Services.AddDbContext<ValmContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MariaDBConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//// Configurar el puerto 5000
//if (!builder.Environment.IsDevelopment())
//{
//    builder.WebHost.ConfigureKestrel(serverOptions =>
//    {
//        serverOptions.ListenAnyIP(5000);
//    });
//}


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

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


// Aplicamos de manera automatica las actualizaciones de la base de datos
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
        var logger = loggerfactory.CreateLogger<Program>();
        logger.LogError(ex, "ocurrio Un error durante la migración");
    }
}

//app.UseCors("CorsPolicy");
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
