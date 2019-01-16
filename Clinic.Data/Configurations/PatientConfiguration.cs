using Clinic.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.Data.Configurations
{
    public class PatientConfiguration : EntityTypeConfiguration<Patient>
    {
        public PatientConfiguration()
        {
            ToTable("Patients");
            HasKey(p => p.Id);

            HasMany(p => p.Bookings)
                .WithRequired(b => b.Patient)
                .HasForeignKey(b => b.PatientId);
        }
    }
}
