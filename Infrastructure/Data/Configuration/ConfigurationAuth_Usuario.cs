using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class ConfigurationAuth_Usuario : IEntityTypeConfiguration<Auth_Usuario>
    {
        public void Configure(EntityTypeBuilder<Auth_Usuario> builder)
        {
            builder.ToTable("Auth_Usuarios");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Usuario).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Nombre).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Correo).HasMaxLength(255);
            builder.Property(u => u.Dni).HasMaxLength(255);

            // Relación con Usuarios_Roles
            builder.HasMany(u => u.Auth_UsuarioRoles)
                .WithOne(ur => ur.Auth_Usuario)
                .HasForeignKey(ur => ur.UsuarioId)
                .IsRequired();
        }
    }
}
