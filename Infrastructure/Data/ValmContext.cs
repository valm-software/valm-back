using Core.Entities;
using Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class ValmContext : DbContext
    {
        public ValmContext(DbContextOptions<ValmContext> options) : base(options)
        {
        }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }

        public DbSet<AuthUsuario> AuthUsuarios { get; set; }
        public DbSet<AuthRol> AuthRoles { get; set; }
        public DbSet<AuthPolitica> AuthPoliticas { get; set; }
        public DbSet<AuthRolPolitica> AuthRolesPoliticas { get; set; }
        public DbSet<AuthUsuarioRol> AuthUsuariosRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "utf8mb4_general_ci");


            modelBuilder.ApplyConfiguration(new MarcaConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoConfiguration());

            modelBuilder.ApplyConfiguration(new AuthUsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new AuthRolConfiguration());
            modelBuilder.ApplyConfiguration(new AuthPoliticaConfiguration());
            modelBuilder.ApplyConfiguration(new AuthRolPoliticaConfiguration());
            modelBuilder.ApplyConfiguration(new AuthUsuarioRolConfiguration());


            //Con este aplica todas las configuracion sin necesidad de hacer referencia de manera individual
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        }
    }
}
