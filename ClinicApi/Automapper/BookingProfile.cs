using AutoMapper;
using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using ClinicApi.Models.Booking;
using ClinicApi.Models.Document;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ClinicApi.Automapper
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingDto, PatientBookingModel>()
                .ForMember(p => p.ClinicianName, options => options.MapFrom(b => b.Clinician.Name))
                .ForMember(p => p.ClinicianSurname, options => options.MapFrom(b => b.Clinician.Surname))
                .ForMember(p => p.ClinicianRate, options => options.MapFrom(b => b.Clinician.Rate));
            CreateMap<PatientBookingModel, BookingDto>();

            CreateMap<BookingDto, ClinicianBookingModel>()
                .ForMember(p => p.PatientName, options => options.MapFrom(b => b.Patient.Name))
                .ForMember(p => p.PatientSurname, options => options.MapFrom(b => b.Patient.Surname))
                .ForMember(p => p.PatientRegion, options => options.MapFrom(b => b.Patient.Region));
            CreateMap<ClinicianBookingModel, BookingDto>();

            CreateMap<Booking, PatientBookingModel>()
                .ForMember(p => p.ClinicId, options => options.MapFrom(b => b.ClinicClinician.ClinicId))
                .ForMember(p => p.ClinicName, options => options.MapFrom(b => b.ClinicClinician.Clinic.Name))
                .ForMember(p => p.ClinicianId, options => options.MapFrom(b => b.ClinicClinician.ClinicianId))
                .ForMember(p => p.ClinicianRate, options => options.MapFrom(b => b.ClinicClinician.Clinician.Rate))
                .ForMember(p => p.ClinicianName, options => options.MapFrom(b => b.ClinicClinician.Clinician.Name))
                .ForMember(p => p.ClinicianSurname, options => options.MapFrom(b => b.ClinicClinician.Clinician.Surname));
            CreateMap<PatientBookingModel, Booking>()
                .ForMember(b => b.Patient, options => options.Ignore())
                .ForMember(b => b.ClinicClinician, options => options.Ignore());

            CreateMap<Booking, UpdateBookingModel>()
                .ForMember(p => p.ClinicId, options => options.MapFrom(b => b.ClinicClinician.ClinicId))
                .ForMember(p => p.ClinicianId, options => options.MapFrom(b => b.ClinicClinician.ClinicianId));
            CreateMap<UpdateBookingModel, Booking>()
                .ForMember(b => b.Documents, options => options.Ignore())
                .ForMember(b => b.Patient, options => options.Ignore())
                .ForMember(b => b.ClinicClinician, options => options.Ignore());

            CreateMap<NameValueCollection, PatientBookingModel>()
                .ForMember(p => p.ClinicianId, options => options.MapFrom(c => c[nameof(PatientBookingModel.ClinicianId)]))
                .ForMember(p => p.ClinicianName, options => options.MapFrom(c => c[nameof(PatientBookingModel.ClinicianName)]))
                .ForMember(p => p.ClinicianRate, options => options.MapFrom(c => c[nameof(PatientBookingModel.ClinicianRate)]))
                .ForMember(p => p.ClinicianSurname, options => options.MapFrom(c => c[nameof(PatientBookingModel.ClinicianSurname)]))
                .ForMember(p => p.ClinicId, options => options.MapFrom(c => c[nameof(PatientBookingModel.ClinicId)]))
                .ForMember(p => p.ClinicName, options => options.MapFrom(c => c[nameof(PatientBookingModel.ClinicName)]))
                .ForMember(p => p.Documents, options => options.MapFrom(
                    c => JsonConvert.DeserializeObject<ICollection<DocumentModel>>(c[nameof(PatientBookingModel.Documents)])))
                .ForMember(p => p.Id, options => options.MapFrom(c => c[nameof(PatientBookingModel.Id)]))
                .ForMember(p => p.Name, options => options.MapFrom(c => c[nameof(PatientBookingModel.Name)]))
                .ForMember(p => p.Reciept, options => options.MapFrom(c => c[nameof(PatientBookingModel.Reciept)]));

            CreateMap<NameValueCollection, UpdateBookingModel>()
                .ForMember(p => p.ClinicianId, options => options.MapFrom(c => c[nameof(PatientBookingModel.ClinicianId)]))
                .ForMember(p => p.ClinicId, options => options.MapFrom(c => c[nameof(PatientBookingModel.ClinicId)]))
                .ForMember(p => p.Documents, options => options.MapFrom(
                    c => JsonConvert.DeserializeObject<ICollection<DocumentModel>>(c[nameof(PatientBookingModel.Documents)])))
                .ForMember(p => p.Id, options => options.MapFrom(c => c[nameof(PatientBookingModel.Id)]))
                .ForMember(p => p.Name, options => options.MapFrom(c => c[nameof(PatientBookingModel.Name)]))
                .ForMember(p => p.Reciept, options => options.MapFrom(c => c[nameof(PatientBookingModel.Reciept)]));

        }
    }
}