namespace Clinic.Core.DtoModels.Account
{
    public class PatientRegistrationDto: UserRegistrationDto
    {
        public string Location { get; set; }
    }
}
