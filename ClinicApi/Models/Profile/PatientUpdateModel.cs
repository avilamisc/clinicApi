using System;

namespace ClinicApi.Models.Profile
{
    public class PatientUpdateModel : ProfileUpdateModel
    {
        public DateTime BornDate { get; set; }
    }
}