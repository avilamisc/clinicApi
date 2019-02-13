using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ClinicApi.Infrastructure
{
    /*public class TokenValidationHandler : DelegatingHandler
    {
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

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;

            if (!TryRetrieveToken(request, out string token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["SecurityKey"]));

                var handler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = securityKey,
                    LifetimeValidator = LifeTimeValidator
                };

                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out SecurityToken securityToken);

                return base.SendAsync(request, cancellationToken);
            }
            catch(SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }
            catch (Exception)
            {
                var resultTask = ExternalTokenValidator.VerifyExternalAccessToken("Google", token);
                resultTask.Wait();

                var res = resultTask.Result;

                statusCode = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
        }

        private bool LifeTimeValidator(DateTime? notBefore, DateTime? expireTime,
            SecurityToken securityToken, TokenValidationParameters parameters)
        {
            if (expireTime != null)
            {
                if (DateTime.UtcNow < expireTime) return true;
            }

            return false;
        }
    }*/
}