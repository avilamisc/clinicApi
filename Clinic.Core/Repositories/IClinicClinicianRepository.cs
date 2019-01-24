using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface IClinicClinicianRepository : IRepository<ClinicClinician>
    {
        Task<ClinicClinician> GetClinicClinicianAsync(int clinicId, int clinicianId);

        Task<IEnumerable<ClinicWithDistanceDto>> GetClinicClinicianSortedByDistance(DbGeography distanceFrom);

        Task<IEnumerable<ClinicWithDistanceDto>> GetClinicClinicianSortedByDistanceV2(DbGeography distanceFrom);

        Task<IEnumerable<ClinicWithDistanceDto>> GetClinicClinicianSortedByDistanceV3(DbGeography distanceFrom);
    }
}
