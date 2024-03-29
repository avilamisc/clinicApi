﻿using System;
using System.Collections.Generic;

using Clinic.Core.Enums;

namespace Clinic.Core.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public string ImageUrl { get; set; }
        public DateTime RegistrationDate { get; set; }

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
