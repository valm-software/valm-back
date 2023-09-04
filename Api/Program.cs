using Api.Extensions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());
// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.AddAplicacionServices();

builder.Services.AddControllers();

    // Este es actualizado y autodetecta la version de mariaDb
builder.Services.AddDbContext<ValmContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MariaDBConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


/*
 * Esta version es para una vercion especifica de mariadb
builder.Services.AddDbContext<ValmContext>(options =>
{
    var serverVersion = new MariaDbServerVersion(new Version(11, 0, 2));
    options.UseMySql(builder.Configuration.GetConnectionString("MariaDBConnection"), serverVersion);

});   
*/


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplicamos de manera automatica las actualizaciones de la base de datos
using (var scope = app.Services.CreateScope())
{
    var services =scope.ServiceProvider;
    var loggerfactory =services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context =services.GetRequiredService<ValmContext>();
        await context.Database.MigrateAsync();
        await ValmContextSeed.SeedAsync(context, loggerfactory);
    }
    catch (Exception ex)
    {
        var logger = loggerfactory.CreateLogger<Program>();
        logger.LogError(ex, "ocurrio Un error durante la migración");
    }
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
