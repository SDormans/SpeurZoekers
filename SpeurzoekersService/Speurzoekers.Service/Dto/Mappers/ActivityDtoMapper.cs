using AutoMapper;
using Speurzoekers.Common.Domain;

namespace Speurzoekers.Service.Dto.Mappers
{
    public class ActivityDtoMapper : Profile
    {
        public ActivityDtoMapper()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<ActivityDto, Activity>();
            CreateMap<ActivitySummary, ActivitySummaryDto>();
            CreateMap<ActivitySummaryDto, ActivitySummary>();
        }
    }
}
