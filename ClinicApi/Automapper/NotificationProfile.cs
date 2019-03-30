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
                .ForMember(n => n.Message, options => options.MapFrom(nDto => nDto.Content))
                .ForMember(n => n.CreationDate, options => options.MapFrom(nDto => nDto.CreatedDate));
            CreateMap<NotificationModel, NotificationDto>()
                .ForMember(n => n.Content, options => options.MapFrom(nModel => nModel.Message))
                .ForMember(n => n.CreatedDate, options => options.MapFrom(nModel => nModel.CreationDate));

            CreateMap<CreateNotificationModel, CreateNotificationDto>()
                .ForMember(nDto => nDto.AuthorId, options => options.Ignore())
                .ForMember(nDto => nDto.IsRead, options => options.Ignore())
                .ForMember(nDto => nDto.CreationDate, options => options.MapFrom(nModel => nModel.CreationDate));
            CreateMap<CreateNotificationDto, CreateNotificationModel>()
                .ForMember(nModel => nModel.CreationDate, options => options.MapFrom(nDto => nDto.CreationDate));

            CreateMap<Notification, NotificationModel>()
                .ForMember(nModel => nModel.Message, options => options.MapFrom(n => n.Content))
                .ForMember(nModel => nModel.CreationDate, options => options.MapFrom(n => n.CreationDate));
            CreateMap<NotificationModel, Notification>()
                .ForMember(n => n.CreationDate, options => options.MapFrom(nModel => nModel.CreationDate));

            CreateMap<UpdateNotificationModel, Notification>()
                .ForMember(n => n.Author, options => options.Ignore())
                .ForMember(n => n.User, options => options.Ignore())
                .ForMember(n => n.IsRead, options => options.MapFrom(nModel => nModel.IsRead.Value));
        }
    }
}