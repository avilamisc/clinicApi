using AutoMapper;
using Clinic.Core.DtoModels.Account;
using ClinicApi.Models.Account.Registration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ClinicApi.Automapper
{
    public class AccountProfile: Profile
    {
        public AccountProfile()
        {
            CreateMap<NameValueCollection, PatientRegisterModel>()
                .ForMember(p => p.UserName, options => options.MapFrom(c => c[nameof(PatientRegisterModel.UserName)]))
                .ForMember(p => p.UserMail, options => options.MapFrom(c => c[nameof(PatientRegisterModel.UserMail)]))
                .ForMember(p => p.Password, options => options.MapFrom(c => c[nameof(PatientRegisterModel.Password)]))
                .ForMember(p => p.Location, options => options.MapFrom(c => c[nameof(PatientRegisterModel.Location)]));

            CreateMap<NameValueCollection, ClinicianRegisterModel>()
                .ForMember(p => p.UserName, options => options.MapFrom(c => c[nameof(ClinicianRegisterModel.UserName)]))
                .ForMember(p => p.UserMail, options => options.MapFrom(c => c[nameof(ClinicianRegisterModel.UserMail)]))
                .ForMember(p => p.Password, options => options.MapFrom(c => c[nameof(ClinicianRegisterModel.Password)]))
                .ForMember(p => p.ClinicsId, options => options.MapFrom(
                    c => JsonConvert.DeserializeObject<ICollection<int>>(c[nameof(ClinicianRegisterModel.ClinicsId)])));

            CreateMap<ClinicianRegisterModel, ClinicianRegistrationDto>()
                .ForMember(dto => dto.Email, options => options.MapFrom(m => m.UserMail))
                .ForMember(dto => dto.Name, options => options.MapFrom(m => m.UserName.Split(' ')[0]))
                .ForMember(dto => dto.Surname, options => options.MapFrom(m => m.UserName.Split(' ')[1]))
                .ForMember(dto => dto.Role, options => options.Ignore())
                .ForMember(dto => dto.RelatedClinics, options => options.Ignore())
                .ForMember(dto => dto.PasswordHash, options => options.Ignore());

            CreateMap<PatientRegisterModel, PatientRegistrationDto>()
                .ForMember(dto => dto.Email, options => options.MapFrom(m => m.UserMail))
                .ForMember(dto => dto.Name, options => options.MapFrom(m => m.UserName.Split(' ')[0]))
                .ForMember(dto => dto.Surname, options => options.MapFrom(m => m.UserName.Split(' ')[1]))
                .ForMember(dto => dto.Role, options => options.Ignore())
                .ForMember(dto => dto.PasswordHash, options => options.Ignore());
        }
    }
}