using AutoMapper;
using Clinic.Core.Entities;
using Clinic.Core.DtoModels;

namespace Clinic.Core.Automapper
{
    public class ClinicClinicianProfile : Profile
    {
        public ClinicClinicianProfile()
        {
            CreateMap<ClinicClinician, ClinicClinicianDto>();
            CreateMap<ClinicClinicianDto, ClinicClinician>()
                .ForMember(cc => cc.Bookings, options => options.Ignore())
                .ForMember(cc => cc.Clinic, options => options.Ignore())
                .ForMember(cc => cc.Clinician, options => options.Ignore());
        }
    }
}
