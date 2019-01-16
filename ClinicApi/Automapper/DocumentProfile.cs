using AutoMapper;
using Clinic.Core.DtoModels;
using Clinic.Core.Entities;
using ClinicApi.Models.Document;

namespace ClinicApi.Automapper
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<DocumentModel, Document>()
                .ForMember(d => d.Booking, options => options.Ignore())
                .ForMember(d => d.User, options => options.Ignore());
            CreateMap<Document, DocumentModel>();

            CreateMap<DocumentModel, DocumentDto>();
            CreateMap<DocumentDto, DocumentModel>();
        }
    }
}