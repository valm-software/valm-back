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
        public DbSet<Auth_Usuario> Auth_Usuarios { get; set; }
        public DbSet<Auth_Rol> Auth_Roles { get; set; }
        public DbSet<Auth_Permiso> Auth_Permisos { get; set; }
        public DbSet<Auth_RolPermiso> Auth_RolesPermisos { get; set; }
        public DbSet<Auth_UsuarioRol> Auth_UsuariosRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "utf8mb4_general_ci");


            modelBuilder.ApplyConfiguration(new ConfigurationMarca());
            modelBuilder.ApplyConfiguration(new ConfigurationCategoria());
            modelBuilder.ApplyConfiguration(new ConfigurationProducto());

            modelBuilder.ApplyConfiguration(new ConfigurationAuth_Usuario());
            modelBuilder.ApplyConfiguration(new ConfigurationAuth_Rol());
            modelBuilder.ApplyConfiguration(new ConfigurationAuth_RolPermiso());
            modelBuilder.ApplyConfiguration(new ConfigurationAuth_RolPermiso());
            modelBuilder.ApplyConfiguration(new ConfigurationAuth_UsuarioRol());


            //Con este aplica todas las configuracion sin necesidad de hacer referencia de manera individual
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        }
    }
}
