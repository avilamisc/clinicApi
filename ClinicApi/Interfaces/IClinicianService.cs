using ClinicApi.Models;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicianService
    {
        Task<ApiResponse> GetCliniciansForClinic(int? clinicId);
    }
}
