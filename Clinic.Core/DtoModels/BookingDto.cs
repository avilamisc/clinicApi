using System;
using System.Collections.Generic;

namespace Clinic.Core.DtoModels
{
    public class BookingDto
    {
        public int Id { get; set; }
        public float? Rate { get; set; }
        public short? HeartRate { get; set; }
        public float? Weight { get; set; }
        public short? Height { get; set; }
        public string PatientDescription { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Name { get; set; }
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
