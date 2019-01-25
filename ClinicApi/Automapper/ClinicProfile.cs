using AutoMapper;
using ClinicApi.Models.Clinic;

namespace ClinicApi.Automapper
{
    public class ClinicProfile: Profile 
    {
        public ClinicProfile()
        {
            CreateMap<Clinic.Core.Entities.Clinic, ClinicModel>()
                .ForMember(m => m.Lat, options => options.MapFrom(c => c.Geolocation.Latitude.Value))
                .ForMember(m => m.Long, options => options.MapFrom(c => c.Geolocation.Longitude.Value));
        }
    }
}