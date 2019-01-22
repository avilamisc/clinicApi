using ClinicApi.Models.Document;
using System.Collections.Generic;

namespace ClinicApi.Models.Booking
{
    public class UpdateBookingModel: BookingModel
    {
        public int ClinicId { get; set; }
        public int ClinicianId { get; set; }
        public IEnumerable<DocumentModel> DeletedDocuments { get; set; }
    }
}