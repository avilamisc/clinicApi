using System.Collections.Generic;

namespace Clinic.Core.DtoModels
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string Reciept { get; set; }
        public string Name { get; set; }
        public float? Rate { get; set; }
        public int ClinicId { get; set; }
        public ClinicDto Clinic { get; set; }
        public int ClinicianId { get; set; }
        public ClinicianDto Clinician { get; set; }
        public int ClinicClinicianId { get; set; }
        public int PatientId { get; set; }
        public PatientDto Patient { get; set; }
        public IEnumerable<DocumentDto> Documents { get; set; }
    }
}
