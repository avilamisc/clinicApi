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
                .ForMember(p => p.ClinicianName, options => options.MapFrom(b => $"{b.Clinician.Name} {b.Clinician.Surname}"))
                .ForMember(p => p.ClinicianRate, options => options.MapFrom(b => b.Clinician.Rate))
                .ForMember(p => p.BookingRate, options => options.MapFrom(b => b.Rate));
            CreateMap<PatientBookingModel, BookingDto>();

            CreateMap<BookingDto, ClinicianBookingModel>()
                .ForMember(p => p.PatientName, options => options.MapFrom(b => $"{b.Patient.Name} {b.Patient.Surname}"))
                .ForMember(p => p.PatientLocation, options => options.MapFrom(b => b.Patient.Location));
            CreateMap<ClinicianBookingModel, BookingDto>();

            CreateMap<Booking, PatientBookingModel>()
                .ForMember(p => p.ClinicId, options => options.MapFrom(b => b.ClinicClinician.ClinicId))
                .ForMember(p => p.ClinicName, options => options.MapFrom(b => b.ClinicClinician.Clinic.ClinicName))
                .ForMember(p => p.ClinicianId, options => options.MapFrom(b => b.ClinicClinician.ClinicianId))
                .ForMember(p => p.ClinicianRate, options => options.MapFrom(b => b.ClinicClinician.Clinician.Rate))
                .ForMember(p => p.ClinicianName, options =>
                    options.MapFrom(b => $"{b.ClinicClinician.Clinician.Name} {b.ClinicClinician.Clinician.Surname}"));
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
                .ForMember(p => p.ClinicId, options => options.MapFrom(c => c[nameof(PatientBookingModel.ClinicId)]))
                .ForMember(p => p.ClinicName, options => options.MapFrom(c => c[nameof(PatientBookingModel.ClinicName)]))
                .ForMember(p => p.Documents, options => options.MapFrom(
                    c => JsonConvert.DeserializeObject<ICollection<DocumentModel>>(c[nameof(PatientBookingModel.Documents)])))
                .ForMember(p => p.Id, options => options.MapFrom(c => c[nameof(PatientBookingModel.Id)]))
                .ForMember(p => p.Name, options => options.MapFrom(c => c[nameof(PatientBookingModel.Name)]))
                .ForMember(p => p.Reciept, options => options.MapFrom(c => c[nameof(PatientBookingModel.Reciept)]));

            CreateMap<NameValueCollection, UpdateBookingModel>()
                .ForMember(p => p.ClinicianId, options => options.MapFrom(c => c[nameof(UpdateBookingModel.ClinicianId)]))
                .ForMember(p => p.ClinicId, options => options.MapFrom(c => c[nameof(UpdateBookingModel.ClinicId)]))
                .ForMember(p => p.Documents, options => options.MapFrom(
                    c => JsonConvert.DeserializeObject<ICollection<DocumentModel>>(c[nameof(UpdateBookingModel.Documents)])))
                .ForMember(p => p.DeletedDocuments, options => options.MapFrom(
                    c => JsonConvert.DeserializeObject<ICollection<DocumentModel>>(c[nameof(UpdateBookingModel.DeletedDocuments)])))
                .ForMember(p => p.Id, options => options.MapFrom(c => c[nameof(UpdateBookingModel.Id)]))
                .ForMember(p => p.Name, options => options.MapFrom(c => c[nameof(UpdateBookingModel.Name)]))
                .ForMember(p => p.Reciept, options => options.MapFrom(c => c[nameof(UpdateBookingModel.Reciept)]));

            CreateMap<Booking, BookingResultModel>()
                .ForMember(b => b.BookingRate, options => options.MapFrom(m => m.Rate))
                .ForMember(
                    b => b.ClinicianName,
                    options => options.MapFrom(m => $"{m.ClinicClinician.Clinician.Name} {m.ClinicClinician.Clinician.Surname}"))
                .ForMember(b => b.PatientLocation, options => options.MapFrom(m => m.Patient.Location))
                .ForMember(
                    b => b.PatientName,
                    options => options.MapFrom(m => $"{m.Patient.Name} {m.Patient.Surname}"))
                .ForMember(b => b.ClinicName, options => options.MapFrom(m => m.ClinicClinician.Clinic.ClinicName))
                .ForMember(b => b.ClinicId, options => options.MapFrom(m => m.ClinicClinician.ClinicId))
                .ForMember(b => b.ClinicianId, options => options.MapFrom(m => m.ClinicClinician.ClinicianId))
                .ForMember(b => b.ClinicianRate, options => options.MapFrom(m => m.ClinicClinician.Clinician.Rate));
        }
    }
}