using Clinic.Core.Enums;
using ClinicApi.Models;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicClinicianService
    {
        Task<ApiResponse> GetSortdetByDistanceClinicsWithClinicianAsync(double longitude, double latitude, ApiVersion version);
    }
}
