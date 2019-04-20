using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

using ClinicApi.Infrastructure.Auth;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Account;
using ClinicApi.Models.Token;

using Swashbuckle.Swagger.Annotations;

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
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<LoginResultModel>))]
        public async Task<IHttpActionResult> Refresh(RefreshTokenModel refreshTokenModel)
        {
            return Ok(await _tokenService.RefreshTokenAsync(refreshTokenModel));
        }

        [HttpPost]
        [Route("revoke")]
        [HasAccessToken]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IHttpActionResult> Revoke(RevokeTokenModel revokeTokenModel)
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _tokenService.RemoveTokenAsync(identity.Claims, revokeTokenModel));
        }
    }
}