using AutoMapper;
using Speurzoekers.Common.Domain;

namespace Speurzoekers.Data.Entities.Mappers
{
    public class ActivityEntityMapper : Profile
    {
        public ActivityEntityMapper()
        {
            CreateMap<ActivityEntity, Activity>();
            CreateMap<Activity, ActivityEntity>();
            CreateMap<ActivityEntity, ActivitySummary>();
            CreateMap<ActivitySummary, ActivityEntity>();
        }
    }
}
