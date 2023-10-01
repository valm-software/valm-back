using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class AuthRolPermisoConfiguration : IEntityTypeConfiguration<AuthRolPermiso>
    {
        public void Configure(EntityTypeBuilder<AuthRolPermiso> builder)
        {
            builder.ToTable("AuthRolesPermisos");
            builder.HasKey(rp => new { rp.RolId, rp.PermisoId });

            builder.HasOne(rp => rp.AuthRol)
                .WithMany(r => r.AuthRolPermisos)
                .HasForeignKey(rp => rp.RolId);

            builder.HasOne(rp => rp.AuthPermiso)
                .WithMany()
                .HasForeignKey(rp => rp.PermisoId);
        }
    }
}
