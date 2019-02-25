using Clinic.Core.DtoModels.Account;
using Clinic.Core.Entities;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface IPatientRepository: IRepository<Patient>
    {
        Task<Patient> CreatePatientAsync(PatientRegistrationDto registrationDto);
    }
}
