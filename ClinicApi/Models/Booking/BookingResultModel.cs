namespace ClinicApi.Models.Booking
{
    public class BookingResultModel : BookingModel
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public int? PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientLocation { get; set; }
        public int? ClinicianId { get; set; }
        public string ClinicianName { get; set; }
        public float? ClinicianRate { get; set; }
        public float? BookingRate { get; set; }
    }
}