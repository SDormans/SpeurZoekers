using AutoMapper;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Collectible;
using Speurzoekers.Common.Domain.CollectibleType;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Data.Entities.Collectible;
using Speurzoekers.Data.Entities.User;

namespace Speurzoekers.Data.Entities.Mappers
{
    public class CollectibleEntityMapper : Profile
    {
        public CollectibleEntityMapper()
        {
            CreateMap<CollectibleTypeEntity, CollectibleTypeSummary>();

            CreateMap<CollectiblePosition, CollectiblePositionEntity>();
            CreateMap<CollectiblePositionEntity, CollectiblePosition>();

            CreateMap<CollectibleType, CollectibleTypeEntity>();
            CreateMap<CollectibleTypeEntity, CollectibleType>();
            CreateMap<CollectibleTypeChoice, CollectibleTypeChoiceEntity>();
            CreateMap<CollectibleTypeChoiceEntity, CollectibleTypeChoice>();
            CreateMap<CollectibleTypeText, CollectibleTypeTextEntity>();
            CreateMap<CollectibleTypeTextEntity, CollectibleTypeText>();

            CreateMap<UserCollectibleEntity, UserCollectible>();
        }
    }
}
