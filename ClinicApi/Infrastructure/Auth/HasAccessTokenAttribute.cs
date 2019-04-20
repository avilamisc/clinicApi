using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

using Microsoft.IdentityModel.Tokens;

namespace ClinicApi.Infrastructure.Auth
{
    public class HasAccessTokenAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!TryRetrieveToken(actionContext.Request, out string token))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }
            try
            {
                var securityKey = new SymmetricSecurityKey(
                    Encoding.Default.GetBytes(ConfigurationManager.AppSettings["SecurityKey"]));

                var handler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = securityKey
                };

                actionContext.RequestContext.Principal = handler.ValidateToken(
                    token,
                    validationParameters,
                    out SecurityToken securityToken);

                base.OnAuthorization(actionContext);
            }
            catch (SecurityTokenValidationException)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            catch (Exception)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            if (!request.Headers.TryGetValues("Authorization", out IEnumerable<string> authHeaders) || authHeaders.Count() > 1)
            {
                return false;
            }

            var bearerToken = authHeaders.First();
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }
    }
}