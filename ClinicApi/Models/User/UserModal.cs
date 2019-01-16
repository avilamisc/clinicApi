using Clinic.Core.Enums;
using ClinicApi.Models.Token;
using System.Collections.Generic;

namespace ClinicApi.Models.User
{
    public class UserModal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }

        public ICollection<RefreshTokenModel> RefreshTokens { get; set; }
    }
}