﻿using Clinic.Core.Entities;
using ClinicApi.Infrastructure.Constants;
using ClinicApi.Infrastructure;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Account;
using ClinicApi.Models.Token;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Infrastructure.Constants.ValidationErrorMessages;

namespace ClinicApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IApiMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSetings;

        public TokenService(
            IApiMapper mapper,
            IUnitOfWork unitOfWork,
            AppSettings appSetings)
        {
            _mapper = mapper;
            _appSetings = appSetings;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> RefreshTokenAsync(RefreshTokenModel refreshTokenModel)
        {
            var userPrincipal = GetPrincipalFromToken(refreshTokenModel.Token);

            if (!Int32.TryParse(userPrincipal.Claims.Single(c => c.Type == ApiConstants.UserIdClaimName).Value, out int userId))
            {
                return new ApiResponse(HttpStatusCode.BadRequest);
            }

            var user = await _unitOfWork.UserRepository.GetAsync(userId);
            if (user == null) return new ApiResponse(HttpStatusCode.NotFound);

            var refreshToken = await GetUserRefreshTokenAsync(refreshTokenModel.RefreshToken);
            if (refreshToken == null
                    || refreshToken.UserId != userId
                    || refreshToken.ExpiresUtc < DateTime.UtcNow)
            {
                return ApiResponse.ValidationError(AuthErrorMessages.InvalidRefreshToken);
            }

            var newAccessToken = GenerateAccessToken(userPrincipal.Claims);
            var newRefreshToken = GenerateRefreshToken(userId);

            await UpdateRefreshTokenAsync(refreshToken, newRefreshToken.Value, newRefreshToken.ExpiresUtc);

            return ApiResponse.Ok(new LoginResultModel
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken.Value,
                    RefreshTokenExpireTime = newRefreshToken.ExpiresUtc
                });
        }

        public async Task<RefreshToken> GetUserRefreshTokenAsync(string refreshToken)
        {
            return await _unitOfWork.RefreshTokenRepository.GetSingleAsync(r => r.Value == refreshToken);
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken, string newValue, DateTime newExpirationDate)
        {
            refreshToken.Value = newValue;
            refreshToken.ExpiresUtc = newExpirationDate;

            _unitOfWork.RefreshTokenRepository.Update(refreshToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(refreshToken.UserId);
            if (user == null) return false;

            _unitOfWork.RefreshTokenRepository.Create(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public RefreshToken GenerateRefreshToken(int userId, int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var newRefreshToken = new RefreshToken
                {
                    Value = Convert.ToBase64String(randomNumber),
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(_appSetings.RefreshTokenExpirationDays),
                    UserId = userId
                };

                return newRefreshToken;
            }
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSetings.AuthSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_appSetings.AccessTokenExpirtionTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(_appSetings.AuthSecret));

            var parameters = new TokenValidationParameters
            {
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = securityKey
            };

            return handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);
        }
    }
}