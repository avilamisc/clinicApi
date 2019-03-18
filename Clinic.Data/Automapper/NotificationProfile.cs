using AutoMapper;
using Clinic.Core.DtoModels;
using Clinic.Core.Entities;

namespace Clinic.Data.Automapper
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>();
            CreateMap<NotificationDto, Notification>()
                .ForMember(n => n.User, options => options.Ignore())
                .ForMember(n => n.UserId, options => options.Ignore());
        }
    }
}
