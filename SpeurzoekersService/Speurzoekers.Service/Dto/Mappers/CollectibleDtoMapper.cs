using AutoMapper;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Collectible;
using Speurzoekers.Service.Dto.Collectible;

namespace Speurzoekers.Service.Dto.Mappers
{
    public class CollectibleDtoMapper : Profile
    {
        public CollectibleDtoMapper()
        {
            CreateMap<CollectibleType, CollectibleTypeDto>();
            CreateMap<CollectibleTypeChoice, CollectibleTypeChoiceDto>();
            CreateMap<DescriptionText, DescriptionTextDto>();

            CreateMap<CollectiblePosition, CollectiblePositionDto>();
        }
    }
}
