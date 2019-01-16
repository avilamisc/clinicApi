using Clinic.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.Data.Configurations
{
    public class ClinicianConfiguration : EntityTypeConfiguration<Clinician>
    {
        public ClinicianConfiguration()
        {
            ToTable("Clinicians");
            HasKey(c => c.Id);

            HasMany(c => c.ClinicClinicians)
                .WithRequired(cc => cc.Clinician)
                .HasForeignKey(cc => cc.ClinicianId);
        }
    }
}
