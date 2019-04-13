using System;

namespace Clinic.Core.Entities
{
    public class RefreshToken : Entity
    {
        public DateTime  ExpiresUtc { get; set; }
        public string Value { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
