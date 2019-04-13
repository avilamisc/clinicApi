using Clinic.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.Data.Configurations
{
    public class BookingConfiguration : EntityTypeConfiguration<Booking>
    {
        public BookingConfiguration()
        {
            ToTable("Bookings");
            HasKey(b => b.Id);

            Property(b => b.Name).IsRequired();
            Property(b => b.CreationDate).IsRequired();
            Property(b => b.UpdateDate).IsRequired();
            Property(b => b.HeartRate).IsOptional();
            Property(b => b.Height).IsOptional();
            Property(b => b.Weight).IsOptional();
            Property(b => b.PatientDescription).IsOptional();
            Property(b => b.Rate).IsOptional();

            HasRequired(b => b.ClinicClinician)
                .WithMany(cc => cc.Bookings)
                .HasForeignKey(b => b.ClinicClinicianId);

            HasRequired(b => b.Patient)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PatientId);

            HasMany(b => b.Documents)
                .WithOptional(d => d.Booking)
                .HasForeignKey(d => d.BookingId);
        }
    }
}
