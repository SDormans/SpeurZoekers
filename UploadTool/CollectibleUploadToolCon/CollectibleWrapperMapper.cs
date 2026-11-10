using AutoMapper;
using Speurzoekers.Common.Domain.Collectible;

namespace CollectibleUploadToolCon
{
    public class CollectibleWrapperMapper : Profile
    {
        public CollectibleWrapperMapper()
        {
            CreateMap<CollectibleWrapper, CollectibleType>();
        }
    }
}
