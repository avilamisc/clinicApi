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

            return Ok(await _accountService.GetPatientProfile(identity.Claims));
        }

        [HttpGet]
        [BearerAuthorization(Roles = "Clinician")]
        [Route("profile/clinician")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<ClinicianProfileViewModel>))]
        public async Task<IHttpActionResult> ClinicianProfile()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _accountService.GetClinicianProfile(identity.Claims));
        }

        [HttpGet]
        [BearerAuthorization(Roles = "Admin")]
        [Route("profile/admin")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<ClinicProfileViewModel>))]
        public async Task<IHttpActionResult> AdminProfile()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _accountService.GetClinicProfile(identity.Claims));
        }

        //[HttpPut]
        //[Authorize]
        //[Route("profile")]
        //[SwaggerResponse(HttpStatusCode.Accepted, Type = typeof(EditUserProfileModel))]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResult<ProfileViewModel>))]
        //public async Task<IHttpActionResult> EditUserProfile()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;

        //    var result = await _accountService.EditUserProfileAsync(identity.Claims, HttpContext.Current.Request, Url);
        //    if (result.GetCode() == HttpStatusCode.OK) return Ok(result);

        //    return new StatusCodeResult(result.GetCode(), this);
        //}
    }
}