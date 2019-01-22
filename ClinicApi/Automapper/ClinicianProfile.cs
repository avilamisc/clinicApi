using AutoMapper;
using Clinic.Core.DtoModels;
using ClinicApi.Models.Clinician;

namespace ClinicApi.Automapper
{
    public class ClinicianProfile : Profile
    {
        public ClinicianProfile()
        {
            CreateMap<ClinicianDto, ClinicianModel>()
                .ForMember(m => m.Name, options => options.MapFrom(dto => $"{dto.Name} {dto.Surname}"));
        }
    }
}