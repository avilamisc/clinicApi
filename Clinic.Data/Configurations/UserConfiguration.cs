using System.Data.Entity.ModelConfiguration;

using Clinic.Core.Entities;

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
            Property(u => u.RegistrationDate).IsRequired();

            Property(u => u.ImageUrl).IsOptional();

            HasMany(u => u.Documents)
                .WithRequired(d => d.User)
                .HasForeignKey(d => d.UserId);

            HasMany(u => u.RefreshTokens)
                .WithRequired(r => r.User)
                .HasForeignKey(r => r.UserId);

            HasMany(u => u.Notifications)
                .WithRequired(n => n.User)
                .HasForeignKey(n => n.UserId)
                .WillCascadeOnDelete(false);

            HasMany(u => u.CreatedNotifications)
                .WithRequired(n => n.Author)
                .HasForeignKey(n => n.AuthorId)
                .WillCascadeOnDelete(false);
        }
    }
}
