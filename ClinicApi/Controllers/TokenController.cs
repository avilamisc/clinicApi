using ClinicApi.Interfaces;
using ClinicApi.Models.Token;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ClinicApi.Controllers
{
    [RoutePrefix("api/tokens")]
    public class TokenController : ApiController
    {
        private ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IHttpActionResult> Refresh(RefreshTokenModel refreshTokenModel)
        {
            return Ok(await _tokenService.RefreshTokenAsync(refreshTokenModel));
        }
    }
}