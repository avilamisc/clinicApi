using ClinicApi.Models;
using ClinicApi.Models.Pagination;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicService
    {
        Task<ApiResponse> GetClinicByIdAsync(int id);
        Task<ApiResponse> GetAllClinicAsync(PaginationModel paging, double longitude, double latitude);
    }
}
