using AutoMapper;
using Clinic.Core.Entities;
using Clinic.Core.DtoModels;

namespace Clinic.Core.Automapper
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<Patient, PatientDto>();
            CreateMap<PatientDto, Patient>()
                .ForMember(p => p.Documents, options => options.Ignore())
                .ForMember(p => p.Bookings, options => options.Ignore())
                .ForMember(p => p.RefreshTokens, options => options.Ignore());
        }
    }
}
