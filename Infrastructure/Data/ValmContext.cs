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
        public DbSet<ProdProducto> ProdProductos { get; set; }
        public DbSet<UsrPrivilegio> UsrPrivilegios { get; set; }
        public DbSet<UsrRol> UsrRoles { get; set; }
        public DbSet<UsrRolPrivilegio> UsrRolesPrivilegios { get; set; }
        public DbSet<UsrUsuario> UsrUsuarios { get; set; }
        public DbSet<UsrUsuarioRol> UsrUsuariosRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "utf8mb4_general_ci");
            modelBuilder.ApplyConfiguration(new ConfigurationProdProducto());
            modelBuilder.ApplyConfiguration(new ConfigurationUsrPrivilegio());
            modelBuilder.ApplyConfiguration(new ConfigurationUsrRol());
            modelBuilder.ApplyConfiguration(new ConfigurationUsrRolPrivilegio());
            modelBuilder.ApplyConfiguration(new ConfigurationUsrUsuario());
            modelBuilder.ApplyConfiguration(new ConfigurationUsrUsuarioRol());

            modelBuilder.ApplyConfiguration(new MarcaConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoConfiguration());

            //Con este aplica todas las configuracion sin necesidad de hacer referencia de manera individual
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        }
    }
}
