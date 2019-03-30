using Clinic.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.Data.Configurations
{
    public class NotificationConfiguration : EntityTypeConfiguration<Notification>
    {
        public NotificationConfiguration()
        {
            ToTable("Notifications");
            HasKey(n => n.Id);

            Property(n => n.Content).IsRequired();
            Property(n => n.AuthorId).IsRequired();
            Property(n => n.UserId).IsRequired();

            HasRequired(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .WillCascadeOnDelete(false);

            HasRequired(n => n.Author)
                .WithMany(u => u.CreatedNotifications)
                .HasForeignKey(n => n.AuthorId)
                .WillCascadeOnDelete(false);
        }
    }
}
