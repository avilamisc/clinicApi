using System.Collections.Generic;
using System.Security.Claims;

namespace ClinicApi.Interfaces
{
    public interface IServiceBase
    {
        bool CheckUserIdInClaims(IEnumerable<Claim> claims, out int userId);
    }
}
