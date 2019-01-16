using Clinic.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.Data.Configurations
{
    public class ClinicClinicianConfiguration : EntityTypeConfiguration<ClinicClinician>
    {
        public ClinicClinicianConfiguration()
        {
            ToTable("ClinicClinicians");
            HasKey(cc => cc.Id);

            HasRequired(cc => cc.Clinic)
                .WithMany(c => c.ClinicClinicians)
                .HasForeignKey(cc => cc.ClinicId);

            HasRequired(cc => cc.Clinician)
                .WithMany(c => c.ClinicClinicians)
                .HasForeignKey(cc => cc.ClinicianId);

            HasMany(cc => cc.Bookings)
                .WithRequired(b => b.ClinicClinician)
                .HasForeignKey(b => b.ClinicClinicianId);
        }
    }
}
