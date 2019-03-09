namespace ClinicApi.Models.Booking
{
    public class PatientBookingModel : BookingModel
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public int ClinicianId { get; set; }
        public string ClinicianName { get; set; }
        public float ClinicianRate { get; set; }
        public float? BookingRate { get; set; }

        public override bool IsValid()
        {
            if (!base.IsValid()) return false;

            return true;
        }
    }
}