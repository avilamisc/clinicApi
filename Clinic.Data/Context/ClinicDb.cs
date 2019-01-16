using Clinic.Core.Entities;
using System.Data.Entity;
using System.Reflection;

namespace Clinic.Data.Context
{
    public class ClinicDb : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Core.Entities.Clinic> Clinics { get; set; }
        public DbSet<ClinicClinician> ClinicClinicians { get; set; }
        public DbSet<Clinician> Clinicians { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ClinicDb()
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public ClinicDb(string connectionString) : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
