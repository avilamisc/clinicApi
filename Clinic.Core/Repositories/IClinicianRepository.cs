using Clinic.Core.DtoModels;
using Clinic.Core.DtoModels.Account;
using Clinic.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface IClinicianRepository: IRepository<Clinician>
    {
        Task<IEnumerable<ClinicianDto>> GetCliniciansAsync(int? clinicId);
        Clinician CreateClinicianAsync(ClinicianRegistrationDto registrationDto);
    }
}
