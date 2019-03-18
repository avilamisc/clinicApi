using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface INotificationRepository: IRepository<Notification>
    {
        Task<IEnumerable<NotificationDto>> GetNotificationByUserIdAsync(
            int userId,
            PagingDto pagingDto);
    }
}
