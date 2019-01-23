using System.Data.Entity.ModelConfiguration;

namespace Clinic.Data.Configurations
{
    public class ClinicConfiguration : EntityTypeConfiguration<Core.Entities.Clinic>
    {
        public ClinicConfiguration()
        {
            ToTable("Clinics");
            HasKey(c => c.Id);

            Property(c => c.City).IsRequired();
            Property(c => c.Geolocation).IsOptional();

            HasMany(c => c.ClinicClinicians)
                .WithRequired(cc => cc.Clinic)
                .HasForeignKey(cc => cc.ClinicId);
        }
    }
}
