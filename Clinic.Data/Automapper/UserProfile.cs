using AutoMapper;
using Clinic.Core.Entities;
using Clinic.Core.DtoModels;

namespace Clinic.Core.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>()
                .ForMember(u => u.Documents, options => options.Ignore())
                .ForMember(u => u.RefreshTokens, options => options.Ignore());
        }
    }
}
