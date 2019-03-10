using System.Collections.Generic;

namespace ClinicApi.Models.ClinicClinician
{
    public class ClinicWithDistanceModel
    {
        public int Id { get; set; }
        public string ClinicName { get; set; }
        public string City { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double Distance { get; set; }

        public ICollection<ClinicianWithDistanceModel> Clinicians { get; set; }
    }
}