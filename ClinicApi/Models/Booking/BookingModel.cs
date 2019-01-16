using ClinicApi.Models.Document;
using System.Collections.Generic;

namespace ClinicApi.Models.Booking
{
    public class BookingModel
    {
        public int Id { get; set; }
        public string Reciept { get; set; }
        public string Name { get; set; }
        public ICollection<DocumentModel> Documents { get; set; }

        public virtual bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Reciept)) return false;

            if (string.IsNullOrWhiteSpace(Name)) return false;

            if (Documents == null) return false;

            return true;
        }
    }
}