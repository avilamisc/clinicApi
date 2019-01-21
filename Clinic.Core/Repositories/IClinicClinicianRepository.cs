using Clinic.Core.Entities;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface IClinicClinicianRepository : IRepository<ClinicClinician>
    {
        Task<ClinicClinician> GetClinicClinicianAsync(int clinicId, int clinicianId);
    }
}
