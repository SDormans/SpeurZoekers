using AutoMapper;
using Speurzoekers.Common.Domain;

namespace Speurzoekers.Service.Dto.Mappers
{
    public class RegionDtoMapper : Profile
    {
        public RegionDtoMapper()
        {
            CreateMap<Region, RegionDto>();
            CreateMap<RegionDto, Region>();
        }
    }
}
