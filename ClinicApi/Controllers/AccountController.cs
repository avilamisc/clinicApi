using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Account;
using ClinicApi.Models.Account.Registration;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


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

        [HttpGet]
        [Route("api/account/google")]
        public async Task<IHttpActionResult> GoogleLogin(string code)
        {
            var values = new Dictionary<string, string>()
            {
                { "code", code },
                { "client_id", "433233257213-uoailm7olq0d7r1ds4pdlm0thqp4invk.apps.googleusercontent.com"},
                { "client_secret", "lfIBm4ZOriLBWxCNezdAcdbo"},
                { "redirect_uri", "http://localhost:4200/booking" },
                { "grant_type", "authorization_code" }
            };

            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpClient client = new HttpClient();

            var response = await client.PostAsync("https://accounts.google.com/o/oauth2/token", content);

            return Ok(response);
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
    }
}