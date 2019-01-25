using ClinicApi.Models;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicService
    {
        Task<ApiResponse> GetClinicByIdAsync(int id);
        Task<ApiResponse> GetAllClinicAsync(double longitude, double latitude);
    }
}
