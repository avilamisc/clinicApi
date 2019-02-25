using ClinicApi.Models;
using ClinicApi.Models.Account;
using ClinicApi.Models.Account.Registration;
using System.Threading.Tasks;
using System.Web;

namespace ClinicApi.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<LoginResultModel>> AuthenticateAsync(string email, string password);
        Task<ApiResponse<LoginResultModel>> RegisterPatientAsync(HttpRequest request);
        Task<ApiResponse<LoginResultModel>> RegisterClinicianAsync(HttpRequest request);
    }
}
