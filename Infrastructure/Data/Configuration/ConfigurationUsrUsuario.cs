using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    internal class ConfigurationUsrUsuario : IEntityTypeConfiguration<UsrUsuario>
    {
        public void Configure(EntityTypeBuilder<UsrUsuario> builder)
        {
            builder.ToTable("UsrUsuarios");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Usuario)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Contraseña)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
