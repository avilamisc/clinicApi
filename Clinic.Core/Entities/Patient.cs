using System.Collections.Generic;

namespace Clinic.Core.Entities
{
    public class Patient : User
    {
        public string Location /*Region*/ { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public Patient()
        {
            Bookings = new List<Booking>();
        }
    }
}
