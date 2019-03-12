using Clinic.Core.Enums;
using ClinicApi.Models;
using ClinicApi.Models.ClinicClinician;
using ClinicApi.Models.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicClinicianService
    {
        Task<ApiResponse<IEnumerable<ClinicWithDistanceModel>>> GetClinicsWithCliniciansSortdetByDistanceAsync(
            LocationPagingModel pagingModel,
            ApiVersion version);
    }
}
