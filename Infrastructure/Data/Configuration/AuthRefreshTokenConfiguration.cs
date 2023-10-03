

using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    internal class AuthRefreshTokenConfiguration : IEntityTypeConfiguration<AuthRefreshToken>
    {
        public void Configure(EntityTypeBuilder<AuthRefreshToken> builder)
        {
            builder.ToTable("AuthRefreshTokens");
            builder.HasKey(r => r.Id);
        }

    }
}
