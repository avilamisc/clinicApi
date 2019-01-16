using AutoMapper;
using Clinic.Core.DtoModels;

namespace Clinic.Core.Automapper
{
    public class ClinicProfile : Profile
    {
        public ClinicProfile()
        {
            CreateMap<Entities.Clinic, ClinicDto>();
            CreateMap<ClinicDto, Entities.Clinic>()
                .ForMember(c => c.ClinicClinicians, options => options.Ignore());
        }
    }
}
