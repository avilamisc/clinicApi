using AutoMapper;
using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using ClinicApi.Models.Notification;

namespace ClinicApi.Automapper
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationDto, NotificationModel>()
                .ForMember(n => n.Message, options => options.MapFrom(nDto => nDto.Content));
            CreateMap<NotificationModel, NotificationDto>()
                .ForMember(n => n.Content, options => options.MapFrom(nModel => nModel.Message));

            CreateMap<CreateNotificationModel, CreateNotificationDto>()
                .ForMember(nDto => nDto.AuthorId, options => options.Ignore())
                .ForMember(nDto => nDto.IsRead, options => options.Ignore());

            CreateMap<Notification, NotificationModel>()
                .ForMember(nModel => nModel.Message, options => options.MapFrom(n => n.Content));
        }
    }
}