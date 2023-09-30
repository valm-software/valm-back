using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class ConfigurationAuth_RolPermiso : IEntityTypeConfiguration<Auth_RolPermiso>
    {
        public void Configure(EntityTypeBuilder<Auth_RolPermiso> builder)
        {
            builder.ToTable("Auth_RolesPermisos");
            builder.HasKey(rp => new { rp.RolId, rp.PermisoId });

            builder.HasOne(rp => rp.Auth_Rol)
                .WithMany(r => r.Auth_RolPermisos)
                .HasForeignKey(rp => rp.RolId);

            builder.HasOne(rp => rp.Auth_Permiso)
                .WithMany()
                .HasForeignKey(rp => rp.PermisoId);
        }
    }
}
