using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    internal class ConfigurationUsrPrivilegio : IEntityTypeConfiguration<UsrPrivilegio>
    {
        public void Configure(EntityTypeBuilder<UsrPrivilegio> builder)
        {
            builder.ToTable("UsrPrivilegios");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Descripcion)
                .HasMaxLength(250);
        }
    }
}
