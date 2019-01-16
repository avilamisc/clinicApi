using System.Collections.Generic;

namespace Clinic.Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string Reciept { get; set; }
        public string Name { get; set; }

        public int ClinicClinicianId { get; set; }
        public ClinicClinician ClinicClinician { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public ICollection<Document> Documents { get; set; }
    }
}
