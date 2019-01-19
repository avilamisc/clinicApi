using System.Security.Claims;
using System.Threading.Tasks;
using Clinic.Core.Encryption;
using Clinic.Core.UnitOfWork;
using ClinicApi.Infrastructure.Constants;
using ClinicApi.Infrastructure.Constants.ValidationErrorMessages;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Account;

namespace ClinicApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return ApiResponse.ValidationError(AuthErrorMessages.FailedAuthorization);
            }

            var user = await _unitOfWork.UserRepository.GetFirstAsync(u => u.Email == email);
            if (user == null || !Hashing.VerifyPassword(password, user.PasswordHash))
            {
                return ApiResponse.ValidationError(AuthErrorMessages.FailedAuthorization);
            }

            var claims = new Claim[]
            {
                new Claim(ApiConstants.UserIdClaimName, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            await _tokenService.AddRefreshTokenAsync(refreshToken);

            return ApiResponse.Ok(new LoginResultModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Value,
                    RefreshTokenExpireTime = refreshToken.ExpiresUtc,
                    UserId = user.Id
                });
        }
    }
}