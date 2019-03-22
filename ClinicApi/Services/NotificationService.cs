using System;
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

        public async Task<ApiResponse<NotificationModel>> CreateNotificationAsync(
            IEnumerable<Claim> claims, CreateNotificationModel model)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return new ApiResponse<NotificationModel>(HttpStatusCode.BadRequest);
            }

            model.CreationDate = model.CreationDate != null
                ? model.CreationDate
                : DateTime.Now;

            var validationErrorMessage = model.Validate();
            if (validationErrorMessage != null)
            {
                return ApiResponse<NotificationModel>.ValidationError(validationErrorMessage);
            }

            if (userId == model.UserId)
            {
                return ApiResponse<NotificationModel>
                    .ValidationError("Author cannot be a recipient of notification");
            }

            var notificationDto = _mapper.Mapper.Map<CreateNotificationDto>(model);
            notificationDto.AuthorId = userId;
            notificationDto.IsRead = false;

            try
            {
                var entity = _unitOfWork.NotificationRepository.CreateNotification(notificationDto);
                await _unitOfWork.SaveChangesAsync();
                var user = await _unitOfWork.UserRepository.GetSingleAsync(u => u.Id == userId);

                var result = _mapper.Mapper.Map<NotificationModel>(entity);
                result.UserName = $"{user.Name} {user.Surname}";
                result.UserMail = user.Email;

                return ApiResponse<NotificationModel>.Ok(result);
            }
            catch
            {
                return ApiResponse<NotificationModel>.InternalError();
            }
        }

        public async Task<ApiResponse<IEnumerable<NotificationModel>>> GetNotificationsAsync(
            IEnumerable<Claim> claims, PaginationModel pagination)
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