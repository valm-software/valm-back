using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class AuthUsuarioRolConfiguration : IEntityTypeConfiguration<AuthUsuarioRol>
    {
        public void Configure(EntityTypeBuilder<AuthUsuarioRol> builder)
        {
            builder.ToTable("AuthUsuariosRoles");
            builder.HasKey(ur => new { ur.UsuarioId, ur.RolId });

            builder.HasOne(ur => ur.AuthUsuario)
                .WithMany(u => u.AuthUsuarioRoles)
                .HasForeignKey(ur => ur.UsuarioId);

            builder.HasOne(ur => ur.AuthRol)
                .WithMany(r => r.AuthUsuarioRoles)
                .HasForeignKey(ur => ur.RolId);
        }
    }
}
