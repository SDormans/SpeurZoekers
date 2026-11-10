using AutoMapper;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Avatar;
using Speurzoekers.Service.Dto.Avatar;
using System.Collections.Generic;

namespace Speurzoekers.Service.Dto.Mappers
{
    public class AvatarDtoMapper : Profile
    {
        public AvatarDtoMapper()
        {
            CreateMap<AvatarType, AvatarTypeDto>()
                .ForMember(dest => dest.Index, opt => opt.MapFrom(dest => (ulong)dest.Index)); ;
            CreateMap<AvatarTypeNoText, AvatarTypeNoTextDto>()
               .ForMember(dest => dest.Index, opt => opt.MapFrom(dest => (ulong)dest.Index)); ;
            CreateMap<AvatarTypeNoText, AvatarTypeCollectionDto>()
                .ForMember(dest => dest.Index, opt => opt.MapFrom(dest => (ulong)dest.Index)); ;
            CreateMap<KeyValuePair<AvatarTypeNoText, bool>, AvatarTypeCollectionDto>()
                .IncludeMembers(src => src.Key)
                .ForMember(dest => dest.IsCollected, opt => opt.MapFrom(s => s.Value));
            CreateMap<DescriptionText, DescriptionTextDto>();
        }
    }
}
