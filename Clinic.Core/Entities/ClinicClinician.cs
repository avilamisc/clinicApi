using System.Collections.Generic;

namespace Clinic.Core.Entities
{
    public class ClinicClinician : Entity
    {
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }

        public int ClinicianId { get; set; }
        public Clinician Clinician { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public ClinicClinician()
        {
            Bookings = new List<Booking>();
        }
    }
}
