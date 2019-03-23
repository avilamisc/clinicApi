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
            IEnumerable<Claim> claims, PaginationModel pagination);

        Task<ApiResponse<NotificationModel>> CreateNotificationAsync(
            IEnumerable<Claim> claims, CreateNotificationModel model);

        Task<ApiResponse<NotificationModel>> UpdateNotificationAsync(
            IEnumerable<Claim> claims, UpdateNotificationModel model);

        Task<ApiResponse<bool?>> SetReadStateAsync(
            IEnumerable<Claim> claims, UpdatePropertyModel<bool?> updateModel);
    }
}
