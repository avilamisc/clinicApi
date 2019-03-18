using AutoMapper;
using Clinic.Core.DtoModels;
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
        }
    }
}