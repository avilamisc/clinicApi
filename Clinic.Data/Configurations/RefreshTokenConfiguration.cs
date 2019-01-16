using Clinic.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.Data.Configurations
{
    public class RefreshTokenConfiguration : EntityTypeConfiguration<RefreshToken>
    {
        public RefreshTokenConfiguration()
        {
            ToTable("RefreshTokens");
            HasKey(r => r.Id);

            Property(r => r.Value).IsRequired();
            Property(r => r.ExpiresUtc).IsRequired();

            HasRequired(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(r => r.UserId);
        }
    }
}
