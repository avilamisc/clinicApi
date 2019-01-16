using Clinic.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.Data.Configurations
{
    public class DocumentConfiguration : EntityTypeConfiguration<Document>
    {
        public DocumentConfiguration()
        {
            ToTable("Documents");
            HasKey(d => d.Id);

            Property(d => d.Name).IsRequired();

            HasRequired(d => d.User)
                .WithMany(u => u.Documents)
                .HasForeignKey(d => d.UserId);

            HasOptional(d => d.Booking)
                .WithMany(b => b.Documents)
                .HasForeignKey(d => d.BookingId);
        }
    }
}
