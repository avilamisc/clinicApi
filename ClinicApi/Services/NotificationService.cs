using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Clinic.Core.UnitOfWork;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Notification;
using ClinicApi.Models.Pagination;

namespace ClinicApi.Services
{
    public class NotificationService : ServiceBase, INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<NotificationModel>>> GetNotificationsAsync(
            IEnumerable<Claim> claims,
            PaginationModel pagination)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return new ApiResponse<IEnumerable<NotificationModel>>(HttpStatusCode.BadRequest);
            }

            throw new System.NotImplementedException();
        }
    }
}