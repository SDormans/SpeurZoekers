using AutoMapper;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Service.Dto.User;

namespace Speurzoekers.Service.Dto.Mappers
{
    public class UserDtoMapper : Profile
    {
        public UserDtoMapper()
        {
            CreateMap<RegistrationDto, UserRegister>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(s => s.Name));
        }
    }
}
