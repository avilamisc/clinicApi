using ClinicApi.Models;
using ClinicApi.Models.Notification;
using ClinicApi.Models.Pagination;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClinicApi.Interfaces
{
    public interface INotificationService : IServiceBase
    {
        Task<ApiResponse<IEnumerable<NotificationModel>>> GetNotificationsAsync(
            IEnumerable<Claim> claims,
            PaginationModel pagination);
    }
}
