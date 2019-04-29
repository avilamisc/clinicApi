using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Routing;

using Clinic.Core.DtoModels;
using Clinic.Core.DtoModels.Account;
using Clinic.Core.Encryption;
using Clinic.Core.Entities;
using Clinic.Core.Enums;
using Clinic.Core.GeographyExtensions;
using Clinic.Core.UnitOfWork;

using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Infrastructure.Constants;
using ClinicApi.Infrastructure.Constants.ValidationErrorMessages;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Account;
using ClinicApi.Models.Account.Registration;
using ClinicApi.Models.Profile;

namespace ClinicApi.Services
{
    public class AccountService : ServiceBase, IAccountService
    {
        private const int MaxLongitudeValue = 180;
        private const int MaxLatidudeValue = 90;
        private static readonly DateTime MinPatientBornDate = new DateTime(1919, 1, 1);

        private readonly ITokenService _tokenService;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApiMapper _mapper;

        public AccountService(
            ITokenService tokenService,
            IFileService fileService,
            IUnitOfWork unitOfWork,
            IApiMapper mapper)
        {
            _tokenService = tokenService;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PatientProfileViewModel>> GetPatientProfile(IEnumerable<Claim> claims, UrlHelper urlHelper)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return ApiResponse<PatientProfileViewModel>.BadRequest();
            }

            var patient = await _unitOfWork.PatientRepository.GetAsync(userId);
            if (patient == null)
            {
                return ApiResponse<PatientProfileViewModel>.NotFound(ProfileMessages.NotFoundUser);
            }

            var patientModel = _mapper.Mapper.Map<PatientProfileViewModel>(patient);
            patientModel.UserImageUrl = _fileService.GetValidUrl(urlHelper, patient.ImageUrl);

            return ApiResponse<PatientProfileViewModel>.Ok(patientModel);
        }

        public async Task<ApiResponse<ClinicProfileViewModel>> GetClinicProfile(IEnumerable<Claim> claims, UrlHelper urlHelper)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return ApiResponse<ClinicProfileViewModel>.BadRequest();
            }

            var clinic = await _unitOfWork.ClinicRepository.GetAsync(userId);
            if (clinic == null)
            {
                return ApiResponse<ClinicProfileViewModel>.NotFound(ProfileMessages.NotFoundUser);
            }

            var clinicModel = _mapper.Mapper.Map<ClinicProfileViewModel>(clinic);
            clinicModel.UserImageUrl = _fileService.GetValidUrl(urlHelper, clinic.ImageUrl);

            return ApiResponse<ClinicProfileViewModel>.Ok(clinicModel);
        }

        public async Task<ApiResponse<ClinicianProfileViewModel>> GetClinicianProfile(IEnumerable<Claim> claims, UrlHelper urlHelper)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return ApiResponse<ClinicianProfileViewModel>.BadRequest();
            }

            var clinician = await _unitOfWork.ClinicianRepository.GetAsync(userId);
            if (clinician == null)
            {
                return ApiResponse<ClinicianProfileViewModel>.NotFound(ProfileMessages.NotFoundUser);
            }

            var clinicianModel = _mapper.Mapper.Map<ClinicianProfileViewModel>(clinician);
            clinicianModel.UserImageUrl = _fileService.GetValidUrl(urlHelper, clinician.ImageUrl);

            return ApiResponse<ClinicianProfileViewModel>.Ok(clinicianModel);
        }

        public async Task<ApiResponse<PatientProfileViewModel>> UpdatePatientProfile(HttpRequest request,
            IEnumerable<Claim> claims, UrlHelper urlHelper)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return ApiResponse<PatientProfileViewModel>.BadRequest();
            }

            var patient = await _unitOfWork.PatientRepository.GetAsync(userId);
            if (patient == null)
            {
                return ApiResponse<PatientProfileViewModel>.NotFound(ProfileMessages.NotFoundUser);
            }

            var updateModel = _mapper.SafeMap<PatientUpdateModel>(request.Form);
            if (updateModel == null)
            {
                return ApiResponse<PatientProfileViewModel>.BadRequest();
            }

            var validationErrorMessage = ValidateUserProfileModel(updateModel);
            if (validationErrorMessage != null)
            {
                return ApiResponse<PatientProfileViewModel>.ValidationError(validationErrorMessage);
            }

            var oldImage = patient.ImageUrl;
            var newImage = GetUserImageFromRequest(request, ApiConstants.ImageFieldName, ApiConstants.PatientProfileImagesFolder);
            if (newImage != null)
            {
                patient.ImageUrl = newImage;
            }

            patient.BornDate = updateModel.BornDate;
            patient.Email = updateModel.Mail;
            patient.Name = updateModel.Name.Split(' ')[0];
            patient.Surname = updateModel.Name.Split(' ')[1];

            try
            {
                _unitOfWork.PatientRepository.Update(patient);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                if (newImage != null)
                {
                    _fileService.DeleteFile(HostingEnvironment.MapPath(newImage));
                }

                return ApiResponse<PatientProfileViewModel>.InternalError("Cannot update your user");
            }

            if (oldImage != null && newImage != null)
            {
                _fileService.DeleteFile(HostingEnvironment.MapPath(oldImage));
            }

            var resultModel = _mapper.Mapper.Map<PatientProfileViewModel>(patient);
            resultModel.UserImageUrl = _fileService.GetValidUrl(urlHelper, newImage != null ? newImage : oldImage);

            return ApiResponse<PatientProfileViewModel>.Ok(resultModel);
        }

        public async Task<ApiResponse<ClinicProfileViewModel>> UpdateClinicProfile(HttpRequest request,
            IEnumerable<Claim> claims, UrlHelper urlHelper)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return ApiResponse<ClinicProfileViewModel>.BadRequest();
            }

            var clinic = await _unitOfWork.ClinicRepository.GetAsync(userId);
            if (clinic == null)
            {
                return ApiResponse<ClinicProfileViewModel>.NotFound(ProfileMessages.NotFoundUser);
            }

            var updateModel = _mapper.SafeMap<ClinicUpdateModel>(request.Form);
            if (updateModel == null)
            {
                return ApiResponse<ClinicProfileViewModel>.BadRequest();
            }

            var validationErrorMessage = ValidateUserProfileModel(updateModel);
            if (validationErrorMessage != null)
            {
                return ApiResponse<ClinicProfileViewModel>.ValidationError(validationErrorMessage);
            }

            var oldImage = clinic.ImageUrl;
            var newImage = GetUserImageFromRequest(request, ApiConstants.ImageFieldName, ApiConstants.PatientProfileImagesFolder);
            if (newImage != null)
            {
                clinic.ImageUrl = newImage;
            }

            clinic.Email = updateModel.Mail;
            clinic.Name = updateModel.Name.Split(' ')[0];
            clinic.Surname = updateModel.Name.Split(' ')[1];
            clinic.ClinicName = updateModel.ClinicName;

            try
            {
                _unitOfWork.ClinicRepository.Update(clinic);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                if (newImage != null)
                {
                    _fileService.DeleteFile(HostingEnvironment.MapPath(newImage));
                }

                return ApiResponse<ClinicProfileViewModel>.InternalError("Cannot update your user");
            }

            if (oldImage != null && newImage != null)
            {
                _fileService.DeleteFile(HostingEnvironment.MapPath(oldImage));
            }

            var resultModel = _mapper.Mapper.Map<ClinicProfileViewModel>(clinic);
            resultModel.UserImageUrl = _fileService.GetValidUrl(urlHelper, newImage != null ? newImage : oldImage);

            return ApiResponse<ClinicProfileViewModel>.Ok(resultModel);
        }

        public async Task<ApiResponse<ClinicianProfileViewModel>> UpdateClinicianProfile(HttpRequest request,
            IEnumerable<Claim> claims, UrlHelper urlHelper)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return ApiResponse<ClinicianProfileViewModel>.BadRequest();
            }

            var clinician = await _unitOfWork.ClinicianRepository.GetAsync(userId);
            if (clinician == null)
            {
                return ApiResponse<ClinicianProfileViewModel>.NotFound(ProfileMessages.NotFoundUser);
            }

            var updateModel = _mapper.SafeMap<ClinicianUpdateModel>(request.Form);
            if (updateModel == null)
            {
                return ApiResponse<ClinicianProfileViewModel>.BadRequest();
            }

            var validationErrorMessage = ValidateUserProfileModel(updateModel);
            if (validationErrorMessage != null)
            {
                return ApiResponse<ClinicianProfileViewModel>.ValidationError(validationErrorMessage);
            }

            var oldImage = clinician.ImageUrl;
            var newImage = GetUserImageFromRequest(request, ApiConstants.ImageFieldName, ApiConstants.PatientProfileImagesFolder);
            if (newImage != null)
            {
                clinician.ImageUrl = newImage;
            }

            clinician.Email = updateModel.Mail;
            clinician.Name = updateModel.Name.Split(' ')[0];
            clinician.Surname = updateModel.Name.Split(' ')[1];

            try
            {
                _unitOfWork.ClinicianRepository.Update(clinician);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                if (newImage != null)
                {
                    _fileService.DeleteFile(HostingEnvironment.MapPath(newImage));
                }

                return ApiResponse<ClinicianProfileViewModel>.InternalError("Cannot update your user");
            }

            if (oldImage != null && newImage != null)
            {
                _fileService.DeleteFile(HostingEnvironment.MapPath(oldImage));
            }

            var resultModel = _mapper.Mapper.Map<ClinicianProfileViewModel>(clinician);
            resultModel.UserImageUrl = _fileService.GetValidUrl(urlHelper, newImage != null ? newImage : oldImage);

            return ApiResponse<ClinicianProfileViewModel>.Ok(resultModel);
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

        public async Task<ApiResponse<LoginResultModel>> ResetPassword(
            ResetPasswordModel resetModel, IEnumerable<Claim> claims)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return ApiResponse<LoginResultModel>.BadRequest();
            }

            var refreshToken = await _unitOfWork.RefreshTokenRepository.GetFirstAsync(
                rt => rt.Value == resetModel.RefreshToken);
            if (refreshToken == null || refreshToken.UserId != userId)
            {
                return ApiResponse<LoginResultModel>.BadRequest();
            }

            var user = await _unitOfWork.UserRepository.GetAsync(userId);
            if (user == null)
            {
                return ApiResponse<LoginResultModel>.BadRequest("Such user doesn't exist");
            }

            if (!Hashing.VerifyPassword(resetModel.OldPassword, user.PasswordHash))
            {
                return ApiResponse<LoginResultModel>.ValidationError("Wrong password");
            }

            user.PasswordHash = Hashing.HashPassword(resetModel.NewPassword);

            try
            {
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                var loginResult = await GenerateTokenAsync(user);

                return ApiResponse<LoginResultModel>.Ok(loginResult);
            }
            catch (InvalidOperationException)
            {
                return ApiResponse<LoginResultModel>.InternalError();
            }
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

            var profileImage = GetUserImageFromRequest(request, ApiConstants.ImageFieldName, ApiConstants.PatientProfileImagesFolder);

            var clinicianRegistrationDto = _mapper.Mapper.Map<ClinicianRegistrationDto>(registerModel);
            clinicianRegistrationDto.RelatedClinics = clinics;
            clinicianRegistrationDto.PasswordHash = Hashing.HashPassword(registerModel.Password);
            clinicianRegistrationDto.Role = UserRole.Clinician;
            clinicianRegistrationDto.UserImage = profileImage;

            try
            {
                var user = _unitOfWork.ClinicianRepository.CreateClinicianAsync(clinicianRegistrationDto);
                await _unitOfWork.SaveChangesAsync();

                var notificationDtos = CreateNotifications(user.ClinicClinicians, user.Id, $"{user.Name} {user.Surname}");
                _unitOfWork.NotificationRepository.CreateNotifications(notificationDtos);
                await _unitOfWork.SaveChangesAsync();

                var loginResult = await GenerateTokenAsync(user);

                return ApiResponse<LoginResultModel>.Ok(loginResult);
            }
            catch (Exception)
            {
                _fileService.DeleteFile(HostingEnvironment.MapPath(profileImage));
                return ApiResponse<LoginResultModel>.InternalError();
            }
        }

        public async Task<ApiResponse<LoginResultModel>> RegisterAdminAsync(HttpRequest request)
        {
            var registerModel = _mapper.SafeMap<AdminRegisterModel>(request.Form);
            if (registerModel == null)
            {
                return ApiResponse<LoginResultModel>.BadRequest();
            }

            var validationError = ValidateRegistrationModel(registerModel);
            if (validationError != null)
            {
                return validationError;
            }

            var clinicValidationError = ValidateAdminClinicRegistrationModel(registerModel);
            if (clinicValidationError != null)
            {
                return validationError;
            }

            var profileImage = GetUserImageFromRequest(request, ApiConstants.ImageFieldName, ApiConstants.PatientProfileImagesFolder);

            var newClinic = new Clinic.Core.Entities.Clinic
            {
                Name = registerModel.UserName.Split(' ')[0],
                Surname = registerModel.UserName.Split(' ')[1],
                Email = registerModel.UserMail,
                Role = UserRole.Admin,
                PasswordHash = Hashing.HashPassword(registerModel.Password),
                City = registerModel.City,
                Geolocation = GeographyExtensions.CreatePoint(registerModel.Long, registerModel.Lat),
                ClinicName = registerModel.Name,
                RegistrationDate = DateTime.Now,
                ImageUrl = profileImage
            };

            try
            {
                var newUser = _unitOfWork.ClinicRepository.Create(newClinic);
                await _unitOfWork.SaveChangesAsync();

                var loginResult = await GenerateTokenAsync(newUser);

                return ApiResponse<LoginResultModel>.Ok(loginResult);
            }
            catch (Exception)
            {
                _fileService.DeleteFile(HostingEnvironment.MapPath(profileImage));
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
            if (validationError != null)
            {
                return validationError;
            }

            var patientValidationError = ValidatePatientRegistrationModel(registerModel);
            if (patientValidationError != null)
            {
                return patientValidationError;
            }

            var profileImage = GetUserImageFromRequest(request, ApiConstants.ImageFieldName, ApiConstants.PatientProfileImagesFolder);

            var patientRegistrationDto = _mapper.Mapper.Map<PatientRegistrationDto>(registerModel);
            patientRegistrationDto.PasswordHash = Hashing.HashPassword(registerModel.Password);
            patientRegistrationDto.Role = UserRole.Patient;
            patientRegistrationDto.UserImage = profileImage;

            try
            {
                var patient = await _unitOfWork.PatientRepository.CreatePatientAsync(patientRegistrationDto);
                var loginResult = await GenerateTokenAsync(patient);

                return ApiResponse<LoginResultModel>.Ok(loginResult);
            }
            catch (Exception)
            {
                _fileService.DeleteFile(HostingEnvironment.MapPath(profileImage));
                return ApiResponse<LoginResultModel>.InternalError();
            }
        }

        private IEnumerable<CreateNotificationDto> CreateNotifications(IEnumerable<ClinicClinician> clinicClinicians,
            int userId, string userName)
        {
            foreach (var clinicClinician in clinicClinicians)
            {
                yield return new CreateNotificationDto
                {
                    AuthorId = userId,
                    Content = $"{userName} has just assigned for your clinic!",
                    CreationDate = DateTime.Now,
                    IsRead = false,
                    UserId = clinicClinician.ClinicId
                };
            }
        }

        private ApiResponse<LoginResultModel> ValidatePatientRegistrationModel(PatientRegisterModel registerModel)
        {
            if (registerModel.BornDate > DateTime.Now ||
                registerModel.BornDate < MinPatientBornDate)
            {
                return ApiResponse<LoginResultModel>.ValidationError("Wrong born date.");
            }

            return null;
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

        private ApiResponse<LoginResultModel> ValidateAdminClinicRegistrationModel(AdminRegisterModel registerModel)
        {
            if (string.IsNullOrWhiteSpace(registerModel.Name))
            {
                return ApiResponse<LoginResultModel>
                    .ValidationError("Clinic Name is necessary.");
            }

            if (registerModel.Long > MaxLongitudeValue || registerModel.Long < -MaxLongitudeValue)
            {
                return ApiResponse<LoginResultModel>
                    .ValidationError("Longitude is incorrect.");
            }

            if (registerModel.Lat > MaxLatidudeValue || registerModel.Lat < -MaxLatidudeValue)
            {
                return ApiResponse<LoginResultModel>
                    .ValidationError("Latitude is incorrect.");
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

        private string GetUserImageFromRequest(HttpRequest request, string imageFieldName, string imagesFolder)
        {
            string fileUrl = null;

            if (request.Files.Count > 0)
            {
                var image = request.Files.Get(imageFieldName);
                if (image != null)
                {
                    fileUrl = _fileService.UploadFile(imagesFolder, image);
                }
            }

            return fileUrl;
        }

        private string ValidateUserProfileModel(ProfileUpdateModel profileViewModel)
        {
            if (string.IsNullOrWhiteSpace(profileViewModel.Name))
            {
                return "Name is required";
            }

            if (profileViewModel.Name.Split(' ').Count() != 2)
            {
                return "Wrong name format";
            }

            if (string.IsNullOrWhiteSpace(profileViewModel.Mail))
            {
                return "Mail is required";
            }

            return null;
        }
    }
}