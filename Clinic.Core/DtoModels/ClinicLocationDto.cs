using System.Collections.Generic;

namespace Clinic.Core.DtoModels
{
    public class ClinicLocationDto
    {
        public int Id { get; set; }
        public string ClinicName { get; set; }
        public string City { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double Distance { get; set; }
        public IEnumerable<ClinicianDto> Clinicians { get; set; }
    }
}
