using Clinic.Core.Enums;
using ClinicApi.Models;
using ClinicApi.Models.ClinicClinician;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicClinicianService
    {
        Task<ApiResponse<IEnumerable<ClinicClinicianBase>>> GetClinicsWithCliniciansSortdetByDistanceAsync(
            double longitude,
            double latitude,
            ApiVersion version);
    }
}
