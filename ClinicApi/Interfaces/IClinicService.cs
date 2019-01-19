using ClinicApi.Models;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IClinicService
    {
        Task<ApiResponse> GetAllClinic();
    }
}
