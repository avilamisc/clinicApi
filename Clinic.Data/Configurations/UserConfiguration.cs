using Clinic.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.Data.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");
            HasKey(u => u.Id);

            Property(u => u.Name).IsRequired();
            Property(u => u.Surname).IsRequired();
            Property(u => u.Email).IsRequired();
            Property(u => u.PasswordHash).IsRequired();

            HasMany(u => u.Documents)
                .WithRequired(d => d.User)
                .HasForeignKey(d => d.UserId);

            HasMany(u => u.RefreshTokens)
                .WithRequired(r => r.User)
                .HasForeignKey(r => r.UserId);
        }
    }
}
