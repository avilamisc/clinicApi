using ClinicApi.Models;
using ClinicApi.Models.Clinic;
using ClinicApi.Models.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicService
    {
        Task<ApiResponse<ClinicModel>> GetClinicByIdAsync(int id);
        Task<ApiResponse<IEnumerable<ClinicModel>>> GetAllClinicAsync(PaginationModel paging, double longitude, double latitude);
    }
}
