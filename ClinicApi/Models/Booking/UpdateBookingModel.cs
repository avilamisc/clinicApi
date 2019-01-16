namespace ClinicApi.Models.Booking
{
    public class UpdateBookingModel: BookingModel
    {
        public int ClinicId { get; set; }
        public int ClinicianId { get; set; }
    }
}