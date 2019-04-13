using System;

namespace Clinic.Core.Entities
{
    public class Notification : Entity
    {
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsRead { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }
    }
}
