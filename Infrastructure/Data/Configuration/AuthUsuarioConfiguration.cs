using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class AuthUsuarioConfiguration : IEntityTypeConfiguration<AuthUsuario>
    {
        public void Configure(EntityTypeBuilder<AuthUsuario> builder)
        {
            builder.ToTable("AuthUsuarios");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Usuario).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Nombre).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Correo).HasMaxLength(255);
            builder.Property(u => u.Dni).HasMaxLength(255);

            // Relación con Usuarios_Roles
            builder.HasMany(u => u.AuthUsuarioRoles)
                .WithOne(ur => ur.AuthUsuario)
                .HasForeignKey(ur => ur.UsuarioId)
                .IsRequired();
        }
    }
}
