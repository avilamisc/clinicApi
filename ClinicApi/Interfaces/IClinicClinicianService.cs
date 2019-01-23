using ClinicApi.Models;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicClinicianService
    {
        Task<ApiResponse> GetSortdetByDistanceClinicsWithClinician(double longitude, double latitude);
    }
}
