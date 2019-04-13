using System;

namespace Clinic.Core.DtoModels.Account
{
    public class PatientRegistrationDto: UserRegistrationDto
    {
        public DateTime BornDate { get; set; }
    }
}
