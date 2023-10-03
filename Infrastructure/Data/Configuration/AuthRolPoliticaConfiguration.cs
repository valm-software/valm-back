using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configuration
{
    public class AuthRolPoliticaConfiguration : IEntityTypeConfiguration<AuthRolPolitica>
    {
        public void Configure(EntityTypeBuilder<AuthRolPolitica> builder)
        {
            builder.ToTable("AuthRolesPoliticas");
            builder.HasKey(rp => new { rp.RolId, rp.PoliticaId });

            builder.HasOne(rp => rp.AuthRol)
                .WithMany(r => r.AuthRolPoliticas)
                .HasForeignKey(rp => rp.RolId);

            builder.HasOne(rp => rp.AuthPolitica)
                .WithMany()
                .HasForeignKey(rp => rp.PoliticaId);
        }
    }
}
