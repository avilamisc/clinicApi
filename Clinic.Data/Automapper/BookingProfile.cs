using AutoMapper;
using Clinic.Core.Entities;
using Clinic.Core.DtoModels;

namespace Clinic.Core.Automapper
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingDto, Booking>();
            CreateMap<Booking, BookingDto>()
                .ForMember(bDto => bDto.ClinicianId, options => options.MapFrom(b => b.ClinicClinician.ClinicianId))
                .ForMember(bDto => bDto.Clinician, options => options.MapFrom(b => b.ClinicClinician.Clinician))
                .ForMember(bDto => bDto.ClinicId, options => options.MapFrom(b => b.ClinicClinician.ClinicId))
                .ForMember(bDto => bDto.Clinic, options => options.MapFrom(b => b.ClinicClinician.Clinic));
        }
    }
}
