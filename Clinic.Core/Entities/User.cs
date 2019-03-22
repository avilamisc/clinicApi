using Clinic.Core.Enums;
using System.Collections.Generic;

namespace Clinic.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }

        public ICollection<Document> Documents { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Notification> CreatedNotifications { get; set; }

        public User()
        {
            Documents = new List<Document>();
            RefreshTokens = new List<RefreshToken>();
            Notifications = new List<Notification>();
            CreatedNotifications = new List<Notification>();
        }
    }
}
