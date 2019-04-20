using System;

using Clinic.Core.Enums;

namespace Clinic.Core.DtoModels.Account
{
    public class UserRegistrationDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public string UserImage { get; set; }
    }
}
