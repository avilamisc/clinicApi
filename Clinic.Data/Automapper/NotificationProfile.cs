using AutoMapper;
using Clinic.Core.DtoModels;
using Clinic.Core.Entities;

namespace Clinic.Data.Automapper
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(
                    nDto => nDto.UserName,
                    options => options.MapFrom(n => $"{n.User.Name} {n.User.Surname}"))
                .ForMember(nDto => nDto.UserMail, options => options.MapFrom(n => n.User.Email))
                .ForMember(nDto => nDto.UserId, options => options.MapFrom(n => n.User.Id));
            CreateMap<NotificationDto, Notification>()
                .ForMember(n => n.User, options => options.Ignore())
                .ForMember(n => n.UserId, options => options.Ignore());

            CreateMap<CreateNotificationDto, Notification>()
                .ForMember(n => n.Author, options => options.Ignore())
                .ForMember(n => n.User, options => options.Ignore());
        }
    }
}
