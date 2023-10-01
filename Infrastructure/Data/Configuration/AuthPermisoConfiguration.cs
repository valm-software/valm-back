using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class AuthPermisoConfiguration : IEntityTypeConfiguration<AuthPermiso>
    {
        public void Configure(EntityTypeBuilder<AuthPermiso> builder)
        {
            builder.ToTable("AuthPermisos");
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Modulo).IsRequired().HasMaxLength(255);
        }
    }
}
