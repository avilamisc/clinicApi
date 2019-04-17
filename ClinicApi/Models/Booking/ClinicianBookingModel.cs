using System;

namespace ClinicApi.Models.Booking
{
    public class ClinicianBookingModel : BookingModel
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
    }
}