using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Clinic.Core.DtoModels.Account;
using Clinic.Core.Encryption;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Infrastructure.Constants;
using ClinicApi.Infrastructure.Constants.ValidationErrorMessages;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Account;
using ClinicApi.Models.Account.Registration;

namespace ClinicApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApiMapper _mapper;


        public AccountService(
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IApiMapper mapper)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<LoginResultModel>> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return ApiResponse<LoginResultModel>.ValidationError(AuthErrorMessages.FailedAuthorization);
            }

            var user = await _unitOfWork.UserRepository.GetFirstAsync(u => u.Email == email);
            if (user == null || !Hashing.VerifyPassword(password, user.PasswordHash))
            {
                return ApiResponse<LoginResultModel>.ValidationError(AuthErrorMessages.FailedAuthorization);
            }

            var loginResult = await GenerateTokenAsync(user);

            return ApiResponse<LoginResultModel>.Ok(loginResult);
        }

        public async Task<ApiResponse<LoginResultModel>> RegisterClinicianAsync(HttpRequest request)
        {
            var registerModel = _mapper.SafeMap<ClinicianRegisterModel>(request.Form);
            if (registerModel == null)
            {
                return ApiResponse<LoginResultModel>.BadRequest();
            }

            var validationError = ValidateRegistrationModel(registerModel);
            if (validationError != null) return validationError;

            var clinics = await _unitOfWork.ClinicRepository.GetAsync(c => registerModel.ClinicsId.Contains(c.Id));
            if (clinics.Count() != registerModel.ClinicsId.Count())
            {
                return ApiResponse<LoginResultModel>.ValidationError("Unexisting clinic is selected.");
            }

            var clinicianRegistrationDto = _mapper.Mapper.Map<ClinicianRegistrationDto>(registerModel);
            clinicianRegistrationDto.RelatedClinics = clinics;
            clinicianRegistrationDto.PasswordHash = Hashing.HashPassword(registerModel.Password);
            clinicianRegistrationDto.Role = UserRole.Clinician;

            try
            {
                var user = await _unitOfWork.ClinicianRepository.CreateClinicianAsync(clinicianRegistrationDto);
                var loginResult = await GenerateTokenAsync(user);

                return ApiResponse<LoginResultModel>.Ok(loginResult);
            }
            catch (Exception)
            {
                return ApiResponse<LoginResultModel>.InternalError();
            }
        }

        public async Task<ApiResponse<LoginResultModel>> RegisterPatientAsync(HttpRequest request)
        {
            var registerModel = _mapper.SafeMap<PatientRegisterModel>(request.Form);
            if (registerModel == null)
            {
                return ApiResponse<LoginResultModel>.BadRequest();
            }

            var validationError = ValidateRegistrationModel(registerModel);
            if (validationError != null) return validationError;

            var patientRegistrationDto = _mapper.Mapper.Map<PatientRegistrationDto>(registerModel);
            patientRegistrationDto.PasswordHash = Hashing.HashPassword(registerModel.Password);
            patientRegistrationDto.Role = UserRole.Clinician;

            try
            {
                var patient = await _unitOfWork.PatientRepository.CreatePatientAsync(patientRegistrationDto);
                var loginResult = await GenerateTokenAsync(patient);

                return ApiResponse<LoginResultModel>.Ok(loginResult);
            }
            catch (Exception)
            {
                return ApiResponse<LoginResultModel>.InternalError();
            }
        }

        private ApiResponse<LoginResultModel> ValidateRegistrationModel(UserRegisterModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return ApiResponse<LoginResultModel>.ValidationError("Empty password. It`s required");
            }

            if(string.IsNullOrWhiteSpace(model.UserName) || model.UserName.Split(' ').Length != 2)
            {
                return ApiResponse<LoginResultModel>
                    .ValidationError("User name is empty and must contains name and surname");
            }

            return null;
        }

        private async Task<LoginResultModel> GenerateTokenAsync(User user)
        {
            var claims = new Claim[]
            {
                    new Claim(ApiConstants.UserIdClaimName, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            await _tokenService.AddRefreshTokenAsync(refreshToken);

            return new LoginResultModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Value,
                RefreshTokenExpireTime = refreshToken.ExpiresUtc,
                UserId = user.Id,
                UserName = user.Name
            };
        }
    }
}