using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Context;

namespace Clinic.Data.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(
            ClinicDb context) : base(context)
        {
        }
    }
}
