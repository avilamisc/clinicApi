using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using ClinicApi.Infrastructure.Auth;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Account;
using ClinicApi.Models.Account.Registration;
using ClinicApi.Models.Profile;

using Swashbuckle.Swagger.Annotations;

namespace ClinicApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("login")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<LoginResultModel>))]
        public async Task<IHttpActionResult> Authenticate(AuthenticateModel authenticateDto)
        {
            return Ok(await _accountService.AuthenticateAsync(authenticateDto.Email, authenticateDto.Password));
        }

        [HttpPost]
        [BearerAuthorization]
        [Route("password")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<LoginResultModel>))]
        public async Task<IHttpActionResult> ResetPasswod(ResetPasswordModel resetModel)
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _accountService.ResetPassword(resetModel, identity.Claims));
        }

        [HttpPost]
        [Route("register/patient")]
        [SwaggerResponse(HttpStatusCode.Accepted, Type = typeof(PatientRegisterModel))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<LoginResultModel>))]
        public async Task<IHttpActionResult> RegisterPatient()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Ok(ApiResponse<LoginResultModel>.UnsupportedMediaType());
            }

            return Ok(await _accountService.RegisterPatientAsync(HttpContext.Current.Request));
        }

        [HttpPost]
        [Route("register/clinician")]
        [SwaggerResponse(HttpStatusCode.Accepted, Type = typeof(ClinicianRegisterModel))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<LoginResultModel>))]
        public async Task<IHttpActionResult> RegisterClinician()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Ok(ApiResponse<LoginResultModel>.UnsupportedMediaType());
            }

            return Created(
                "api/account/register/clinician",
                await _accountService.RegisterClinicianAsync(HttpContext.Current.Request));
        }

        [HttpPost]
        [Route("register/admin")]
        [SwaggerResponse(HttpStatusCode.Accepted, Type = typeof(AdminRegisterModel))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<LoginResultModel>))]
        public async Task<IHttpActionResult> RegisterClinicAdmin()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Ok(ApiResponse<LoginResultModel>.UnsupportedMediaType());
            }

            return Created(
                "api/account/register/admin",
                await _accountService.RegisterAdminAsync(HttpContext.Current.Request));
        }

        [HttpGet]
        [BearerAuthorization(Roles = "Patient")]
        [Route("profile/patient")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<ProfileViewModel>))]
        public async Task<IHttpActionResult> PatientProfile()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _accountService.GetPatientProfile(identity.Claims, Url));
        }

        [HttpGet]
        [BearerAuthorization(Roles = "Clinician")]
        [Route("profile/clinician")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<ClinicianProfileViewModel>))]
        public async Task<IHttpActionResult> ClinicianProfile()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _accountService.GetClinicianProfile(identity.Claims, Url));
        }

        [HttpGet]
        [BearerAuthorization(Roles = "Admin")]
        [Route("profile/admin")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<ClinicProfileViewModel>))]
        public async Task<IHttpActionResult> AdminProfile()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _accountService.GetClinicProfile(identity.Claims, Url));
        }

        [HttpPut]
        [BearerAuthorization(Roles = "Patient")]
        [Route("profile/patient")]
        [SwaggerResponse(HttpStatusCode.Accepted, Type = typeof(PatientUpdateModel))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<PatientProfileViewModel>))]
        public async Task<IHttpActionResult> EditPatientProfile()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _accountService.UpdatePatientProfile(HttpContext.Current.Request, identity.Claims, Url));
        }

        [HttpPut]
        [BearerAuthorization(Roles = "Clinician")]
        [Route("profile/clinician")]
        [SwaggerResponse(HttpStatusCode.Accepted, Type = typeof(ClinicianUpdateModel))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<ClinicianProfileViewModel>))]
        public async Task<IHttpActionResult> EditClinicianProfile()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _accountService.UpdateClinicianProfile(HttpContext.Current.Request, identity.Claims, Url));
        }

        [HttpPut]
        [BearerAuthorization(Roles = "Admin")]
        [Route("profile/admin")]
        [SwaggerResponse(HttpStatusCode.Accepted, Type = typeof(ClinicUpdateModel))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<ClinicProfileViewModel>))]
        public async Task<IHttpActionResult> EditClinicProfile()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _accountService.UpdateClinicProfile(HttpContext.Current.Request, identity.Claims, Url));
        }
    }
}