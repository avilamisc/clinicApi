using AutoMapper;
using Clinic.Core.Entities;
using Clinic.Core.DtoModels;

namespace Clinic.Core.Automapper
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<Document, DocumentDto>();
            CreateMap<DocumentDto, Document>()
                .ForMember(d => d.Booking, options => options.UseDestinationValue())
                .ForMember(d => d.BookingId, options => options.UseDestinationValue())
                .ForMember(d => d.User, options => options.UseDestinationValue())
                .ForMember(d => d.UserId, options => options.UseDestinationValue());
        }
    }
}
