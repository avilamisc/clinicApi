using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Clinic.Core.DtoModels;
using Clinic.Core.UnitOfWork;
using ClinicApi.Automapper.Infrastructure;
using ClinicApi.Interfaces;
using ClinicApi.Models;
using ClinicApi.Models.Notification;
using ClinicApi.Models.Pagination;

namespace ClinicApi.Services
{
    public class NotificationService : ServiceBase, INotificationService
    {
        private readonly IApiMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(
            IApiMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
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

            var pagingDto = _mapper.Mapper.Map<PagingDto>(pagination);

            var result = await _unitOfWork.NotificationRepository
                .GetNotificationByUserIdAsync(userId, pagingDto);

            return ApiResponse<IEnumerable<NotificationModel>>.Ok(
                _mapper.Mapper.Map<IEnumerable<NotificationModel>>(result));
        }
    }
}