using ClinicApi.Infrastructure.Auth;
using ClinicApi.Interfaces;
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
        [BearerAuthorization]
        public async Task<IHttpActionResult> Notifications()
        {
            var identity = (ClaimsIdentity)User.Identity;

            return null;
        }
    }
}