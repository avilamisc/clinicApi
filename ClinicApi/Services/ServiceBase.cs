using ClinicApi.Infrastructure.Constants;
using ClinicApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ClinicApi.Services
{
    public class ServiceBase : IServiceBase
    {
        public bool CheckUserIdInClaims(IEnumerable<Claim> claims, out int userId)
        {
            return Int32.TryParse(
                claims.Single(c => c.Type == ApiConstants.UserIdClaimName).Value,
                out userId);
        }
    }
}