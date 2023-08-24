using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;

namespace Api.Extensions
{
    public static class AplicationServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
                    services.AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy", builder =>
                            builder.AllowAnyOrigin()    //WithOrigins("https://dominio.com")
                            .AllowAnyMethod()           //WithMethods("GET","POST")
                            .AllowAnyHeader());         //WithHeaders("accept","contect-type")
                    });

        public static void AddAplicacionServices(this IServiceCollection services)
        {
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IProductoRepository, ProductoRepository>();
            //services.AddScoped<IMarcaRepository, MarcaRepository>();
            //services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

    }
}
