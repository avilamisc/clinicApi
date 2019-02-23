using ClinicApi.Models;
using ClinicApi.Models.Clinician;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicianService
    {
        Task<ApiResponse<IEnumerable<ClinicianModel>>> GetCliniciansForClinic(int? clinicId);
    }
}
