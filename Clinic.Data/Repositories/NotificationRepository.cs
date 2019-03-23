using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using Clinic.Core.Repositories;
using Clinic.Data.Automapper.Infrastructure;
using Clinic.Data.Common;
using Clinic.Data.Context;

namespace Clinic.Data.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly IDataMapper _mapper;

        public NotificationRepository(
            IDataMapper mapper,
            ClinicDb context) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PagingResultDto<NotificationDto>> GetNotificationByUserIdAsync(
            int userId,
            PagingDto pagingDto)
        {
            var result = await _context.Notifications
                .Include(n => n.User)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Id)
                .Paging(pagingDto)
                .ToListAsync();

            var totalAmount = _context.Notifications
                .Where(n => n.UserId == userId)
                .Count();

            return new PagingResultDto<NotificationDto>
            {
                DataColection = _mapper.Mapper.Map<IEnumerable<NotificationDto>>(result),
                TotalCount = totalAmount
            };
        }

        public Notification CreateNotification(CreateNotificationDto dtoModel)
        {
            var entity = _mapper.Mapper.Map<Notification>(dtoModel);
            _context.Notifications.Add(entity);

            return entity;
        }
    }
}
