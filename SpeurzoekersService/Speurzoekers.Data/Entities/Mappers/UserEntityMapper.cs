using AutoMapper;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Data.Entities.Identity;
using Speurzoekers.Data.Entities.User;

namespace Speurzoekers.Data.Entities.Mappers
{
    public class UserEntityMapper : Profile
    {
        public UserEntityMapper()
        {
            CreateMap<UserRegister, ApplicationUser>()
                .ForMember(dest => dest.UserName, 
                    opt => opt.MapFrom(dest => dest.Email))
                .ForMember(dest => dest.UserInfo,
                    opt => opt.MapFrom(src => new UserInfoEntity
                    {
                        DisplayName = src.DisplayName
                    }));
        }
    }
}
