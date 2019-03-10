using AutoMapper;
using Clinic.Core.DtoModels;
using ClinicApi.Models.Clinic;
using ClinicApi.Models.ClinicClinician;
using System.Collections.Generic;

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