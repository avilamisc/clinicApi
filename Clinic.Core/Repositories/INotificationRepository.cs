using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface INotificationRepository: IRepository<Notification>
    {
        Task<PagingResultDto<NotificationDto>> GetNotificationByUserIdAsync(
            int userId,
            PagingDto pagingDto);

        Notification CreateNotification(CreateNotificationDto dtoModel);
        IEnumerable<Notification> CreateNotifications(IEnumerable<CreateNotificationDto> dtoModels);
    }
}
