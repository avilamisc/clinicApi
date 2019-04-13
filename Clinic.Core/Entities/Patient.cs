using System;
using System.Collections.Generic;

namespace Clinic.Core.Entities
{
    public class Patient : User
    {
        public DateTime BornDate { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public int Age
        {
            get { return DateTime.Now.Year - BornDate.Year; }
        }

        public Patient()
        {
            Bookings = new List<Booking>();
        }
    }
}
