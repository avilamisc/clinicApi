using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Account;
using ClinicApi.Models.Account.Registration;

using Swashbuckle.Swagger.Annotations;

namespace ClinicApi.Controllers
{
    public class AccountController : ApiController
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("api/account/login")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<LoginResultModel>))]
        public async Task<IHttpActionResult> Authenticate(AuthenticateModel authenticateDto)
        {
            return Ok(await _accountService.AuthenticateAsync(authenticateDto.Email, authenticateDto.Password));
        }

        [HttpPost]
        [Route("api/account/register/patient")]
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
        [Route("api/account/register/clinician")]
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
        [Route("api/account/register/admin")]
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
    }
}