using System.Collections.Generic;

namespace Clinic.Core.DtoModels.Account
{
    public class ClinicianRegistrationDto: UserRegistrationDto
    {
        public IEnumerable<Entities.Clinic> RelatedClinics { get; set; }
    }
}
