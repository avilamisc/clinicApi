using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using ClinicApi.Models;
using ClinicApi.Models.Account;
using ClinicApi.Models.Profile;

namespace ClinicApi.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<LoginResultModel>> AuthenticateAsync(string email, string password);
        Task<ApiResponse<LoginResultModel>> RegisterPatientAsync(HttpRequest request);
        Task<ApiResponse<LoginResultModel>> RegisterClinicianAsync(HttpRequest request);
        Task<ApiResponse<LoginResultModel>> RegisterAdminAsync(HttpRequest request);
        Task<ApiResponse<ClinicianProfileViewModel>> GetClinicianProfile(IEnumerable<Claim> claims);
        Task<ApiResponse<PatientProfileViewModel>> GetPatientProfile(IEnumerable<Claim> claims);
        Task<ApiResponse<ClinicProfileViewModel>> GetClinicProfile(IEnumerable<Claim> claims);
    }
}
