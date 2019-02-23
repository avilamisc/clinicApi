using Clinic.Core.Entities;
using ClinicApi.Models;
using ClinicApi.Models.Account;
using ClinicApi.Models.Token;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface ITokenService
    {
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken, string newValue, DateTime newExpirationDate);
        Task<ApiResponse<LoginResultModel>> RefreshTokenAsync(RefreshTokenModel refreshTokenModel);
        Task<RefreshToken> GetUserRefreshTokenAsync(string refreshToken);
        Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken);
        RefreshToken GenerateRefreshToken(int userId, int size = 32);
        string GenerateAccessToken(IEnumerable<Claim> claims);
    }
}