using AutoMapper;
using Speurzoekers.Common.Domain;

namespace Speurzoekers.Data.Entities.Mappers
{
    public class RegionEntityMapper : Profile
    {
        public RegionEntityMapper()
        {
            CreateMap<RegionEntity, Region>();
            CreateMap<Region, RegionEntity>();

            CreateMap<RegionEntity, RegionSummary>();
        }
    }
}
