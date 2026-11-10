using AutoMapper;
using Speurzoekers.Common.Domain.Avatar;
using Speurzoekers.Data.Entities.Avatar;

namespace Speurzoekers.Data.Entities.Mappers
{
    public class AvatarEntityMapper : Profile
    {
        public AvatarEntityMapper()
        {
            CreateMap<AvatarTypeEntity, AvatarType>();
            CreateMap<AvatarTypeTextEntity, AvatarTypeText>();

            CreateMap<AvatarTypeEntity, AvatarTypeNoText>();
            CreateMap<AvatarTypeNoText, AvatarTypeEntity>();

            CreateMap<AvatarType, AvatarTypeEntity>();
            CreateMap<AvatarTypeText, AvatarTypeTextEntity >();
        }
    }
}
