using System;

namespace Clinic.Core.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public DateTime  ExpiresUtc { get; set; }
        public string Value { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
