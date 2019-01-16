using AutoMapper;
using Clinic.Core.Entities;
using Clinic.Core.DtoModels;

namespace Clinic.Core.Automapper
{
    public class ClinicianProfile : Profile
    {
        public ClinicianProfile()
        {
            CreateMap<Clinician, ClinicianDto>();
            CreateMap<ClinicianDto, Clinician>()
                .ForMember(c => c.ClinicClinicians, options => options.Ignore())
                .ForMember(c => c.Documents, options => options.Ignore());
        }
    }
}
