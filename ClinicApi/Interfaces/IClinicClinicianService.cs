using ClinicApi.Models;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicClinicianService
    {
        Task<ApiResponse> GetSortdetByDistanceClinicsWithClinician(double longitude, double latitude);
        Task<ApiResponse> GetSortdetByDistanceClinicsWithClinicianV2(double longitude, double latitude);
        Task<ApiResponse> GetSortdetByDistanceClinicsWithClinicianV3(double longitude, double latitude);
    }
}
