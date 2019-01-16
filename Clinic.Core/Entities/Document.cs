namespace Clinic.Core.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
