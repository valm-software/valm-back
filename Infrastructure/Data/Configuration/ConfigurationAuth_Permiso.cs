using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class ConfigurationAuth_Permiso : IEntityTypeConfiguration<Auth_Permiso>
    {
        public void Configure(EntityTypeBuilder<Auth_Permiso> builder)
        {
            builder.ToTable("Auth_Permisos");
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Modulo).IsRequired().HasMaxLength(255);
        }
    }
}
