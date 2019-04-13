using AutoMapper;
using Clinic.Core.DtoModels;

namespace Clinic.Core.Automapper
{
    public class ClinicProfile : Profile
    {
        public ClinicProfile()
        {
            CreateMap<Entities.Clinic, ClinicDto>()
                .ForMember(c => c.Name, options => options.MapFrom(dto => dto.ClinicName))
                .ForMember(c => c.Long, options => options.MapFrom(dto => dto.Geolocation.Longitude))
                .ForMember(c => c.Lat, options => options.MapFrom(dto => dto.Geolocation.Latitude));
            CreateMap<ClinicDto, Entities.Clinic>()
                .ForMember(c => c.ClinicName, options => options.MapFrom(c => c.Name))
                .ForMember(c => c.ClinicClinicians, options => options.Ignore())
                .ForMember(c => c.Geolocation, options => options.Ignore());
        }
    }
}
