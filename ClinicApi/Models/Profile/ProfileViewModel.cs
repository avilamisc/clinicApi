using System;

namespace ClinicApi.Models.Profile
{
    public class ProfileViewModel
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public string UserImageUrl { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}