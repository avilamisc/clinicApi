using AutoMapper;
using Clinic.Core.Entities;
using Clinic.Core.DtoModels;

namespace Clinic.Core.Automapper
{
    public class RefreshTokenProfile : Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<RefreshToken, RefreshTokenDto>();
            CreateMap<RefreshTokenDto, RefreshToken>()
                    .ForMember(rDto => rDto.User, options => options.Ignore());
        }
    }
}
