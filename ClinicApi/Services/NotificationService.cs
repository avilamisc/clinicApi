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

            var validationError = await ValidateNotificationAsync(model, userId);
            if (validationError != null)
            {
                return validationError;
            }

            var notificationDto = _mapper.Mapper.Map<CreateNotificationDto>(model);
            notificationDto.AuthorId = userId;
            notificationDto.IsRead = false;

            try
            {
                var entity = _unitOfWork.NotificationRepository.CreateNotification(notificationDto);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Mapper.Map<NotificationModel>(entity);

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
                return ApiResponse<IEnumerable<NotificationModel>>.BadRequest();
            }

            var pagingDto = _mapper.Mapper.Map<PagingDto>(pagination);

            var result = await _unitOfWork.NotificationRepository
                .GetNotificationByUserIdAsync(userId, pagingDto);

            return ApiResponse<IEnumerable<NotificationModel>>.Ok(
                _mapper.Mapper.Map<IEnumerable<NotificationModel>>(result));
        }

        public async Task<ApiResponse<RemoveResult>> RemoveNotificationAsync(
            IEnumerable<Claim> claims, int id)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return ApiResponse<RemoveResult>.BadRequest();
            }

            var notification = await _unitOfWork.NotificationRepository
                .GetSingleAsync(n => n.Id == id);
            if (notification == null)
            {
                return ApiResponse<RemoveResult>.Ok(
                    RemoveResult.Failed("Unexisitng notification"));
            }

            if (notification.UserId != userId && notification.AuthorId != userId)
            {
                return ApiResponse<RemoveResult>.Ok(
                    RemoveResult.Failed("Don`t have permission"));
            }

            try
            {
                _unitOfWork.NotificationRepository.Remove(notification);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<RemoveResult>.Ok(
                    RemoveResult.Removed("Notification successfuly deleted"));
            }
            catch
            {
                return ApiResponse<RemoveResult>.InternalError();
            }
        }

        public async Task<ApiResponse<bool?>> SetReadStateAsync(
            IEnumerable<Claim> claims, UpdatePropertyModel<bool?> updateModel)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return ApiResponse<bool?>.BadRequest();
            }

            if (!updateModel.Value.HasValue)
            {
                return ApiResponse<bool?>.ValidationError("Is read is required.");
            }

            var entity = await _unitOfWork.NotificationRepository.GetSingleAsync(n => n.Id == updateModel.Id);

            if (entity == null)
            {
                return ApiResponse<bool?>.ValidationError("Unexisting notification");
            }

            if (userId != entity.UserId)
            {
                return ApiResponse<bool?>.ValidationError("Don`t have permission");
            }

            try
            {
                entity.IsRead = updateModel.Value.Value;
                _unitOfWork.NotificationRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool?>.Ok(entity.IsRead);
            }
            catch
            {
                return ApiResponse<bool?>.InternalError("Cannot update entity");
            }
        }

        public async Task<ApiResponse<NotificationModel>> UpdateNotificationAsync(
            IEnumerable<Claim> claims, UpdateNotificationModel model)
        {
            if (!CheckUserIdInClaims(claims, out int userId))
            {
                return new ApiResponse<NotificationModel>(HttpStatusCode.BadRequest);
            }

            var entity = await _unitOfWork.NotificationRepository.GetSingleAsync(n => n.Id == model.Id);
            if (entity == null)
            {
                return ApiResponse<NotificationModel>.ValidationError("Unexisting Notification");
            }

            var validationError = await ValidateNotificationAsync(model, userId);
            if (validationError != null)
            {
                return validationError;
            }

            if (model.CreationDate == null)
            {
                return ApiResponse<NotificationModel>.ValidationError("Creation date is required");
            }

            try
            {
                _mapper.Mapper.Map(model, entity);
                _unitOfWork.NotificationRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<NotificationModel>.Ok(
                    _mapper.Mapper.Map<NotificationModel>(entity));
            }
            catch
            {
                return ApiResponse<NotificationModel>.InternalError();
            }
        }

        private async Task<ApiResponse<NotificationModel>> ValidateNotificationAsync(
            CreateNotificationModel model, int userId)
        {
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

            var user = await _unitOfWork.UserRepository.GetSingleAsync(u => u.Id == model.UserId);
            if (user == null)
            {
                return ApiResponse<NotificationModel>
                    .ValidationError("Such user doesn`t exist");
            }

            return null;
        }
    }
}