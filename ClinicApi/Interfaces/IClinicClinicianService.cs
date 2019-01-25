using Clinic.Core.Enums;
using ClinicApi.Models;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicClinicianService
    {
        Task<ApiResponse> GetClinicsWithCliniciansSortdetByDistanceAsync(double longitude, double latitude, ApiVersion version);
    }
}
