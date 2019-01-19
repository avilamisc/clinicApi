using Clinic.Core.DtoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface IClinicRepository: IRepository<Entities.Clinic>
    {
        Task<IEnumerable<ClinicDto>> GetAllClinicsAsync();
    }
}
