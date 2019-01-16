using ClinicApi.Models;
using ClinicApi.Models.Account;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse> AuthenticateAsync(string email, string password);
    }
}
