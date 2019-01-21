using AutoMapper;
using Clinic.Core.DtoModels;
using ClinicApi.Models.Pagination;

namespace ClinicApi.Automapper
{
    public class PaginationProfile : Profile
    {
        public PaginationProfile()
        {
            CreateMap<PaginationModel, PagingDto>()
                .ForMember(dto => dto.PageNumber, options => options.MapFrom(m => m.PageNumber >= 0 ? m.PageNumber : 0))
                .ForMember(dto => dto.PageSize, options => options.MapFrom(m => m.PageSize));
        }
    }
}