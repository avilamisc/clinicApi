namespace Clinic.Core.Entities
{
    public class Document : Entity
    {
        public string Name { get; set; }
        public string FilePath { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
