using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    internal class ConfigurationUsrRol : IEntityTypeConfiguration<UsrRol>
    {
        public void Configure(EntityTypeBuilder<UsrRol> builder)
        {
            builder.ToTable("UsrRoles");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Descripcion)
                .HasMaxLength(250);
        }
    }
}
