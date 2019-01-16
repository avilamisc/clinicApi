using ClinicApi.Interfaces;
using ClinicApi.Models.Account;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;


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
        public async Task<IHttpActionResult> Authenticate(AuthenticateModel authenticateDto)
        {
            return Ok(await _accountService.AuthenticateAsync(authenticateDto.Email, authenticateDto.Password));
        }
    }
}