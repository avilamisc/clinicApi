using ClinicApi.Infrastructure.Auth;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Notification;
using ClinicApi.Models.Pagination;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClinicApi.Controllers
{
    [RoutePrefix("api/notifications")]
    public class NotificationController : ApiController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("")]
        [BearerAuthorization]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<NotificationModel>>))]
        public async Task<IHttpActionResult> Notifications([FromUri]PaginationModel pagination)
        {
            var identity = (ClaimsIdentity)User.Identity;

            return Ok(await _notificationService.GetNotificationsAsync(identity.Claims, pagination));
        }
    }
}