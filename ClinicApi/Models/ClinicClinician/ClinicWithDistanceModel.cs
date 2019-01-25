namespace ClinicApi.Models.ClinicClinician
{
    public class ClinicWithDistanceModel: ClinicClinicianBase
    {
        public string ClinicName { get; set; }
        public string City { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double Distance { get; set; }
    }
}