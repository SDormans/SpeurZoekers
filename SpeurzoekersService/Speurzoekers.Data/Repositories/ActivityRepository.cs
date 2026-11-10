using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Data.Entities;
using Speurzoekers.Data.Entities.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Speurzoekers.Data.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly SpeurzoekersDbContext _speurzoekersDbContext;
        private readonly IMapper _mapper;

        public ActivityRepository(SpeurzoekersDbContext speurzoekersDbContext, IMapper mapper)
        {
            _speurzoekersDbContext = speurzoekersDbContext;
            _mapper = mapper;
        }

        public async Task<Activity[]> GetActivities()
        {
            var result = await _speurzoekersDbContext.Activities.ToArrayAsync();
            return _mapper.Map<Activity[]>(result);
        }

        public async Task<Activity[]> GetActivities(Season season)
        {
            var result = await _speurzoekersDbContext.Activities
                .Where(x => (x.Season & season) > 0)
                .ToArrayAsync();
            return _mapper.Map<Activity[]>(result);
        }

        public async Task<ActivitySummary[]> GetActivitiesNameAndSeason()
        {
            return await _speurzoekersDbContext.Activities
                .ProjectTo<ActivitySummary>(_mapper.ConfigurationProvider).ToArrayAsync();
        }

        public async Task<Dictionary<Guid, string>> GetActivitiesSummary()
        {
            return await _speurzoekersDbContext.Activities
                .AsNoTracking()
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToDictionaryAsync(k => k.Id, v => v.Name);
        }

        public async Task<Dictionary<Guid, string>> GetActivitiesSummary(Season season)
        {
            return await _speurzoekersDbContext.Activities
                .AsNoTracking()
                .Where(x => (x.Season & season) > 0)
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToDictionaryAsync(k => k.Id, v => v.Name);
        }

        public Task<Dictionary<Guid, string>> GetActivitiesSummary(
            IEnumerable<FilterCriterion> filters, bool orConcatenation, Sorting sorting)
        {
            return _speurzoekersDbContext.GuidSummary
               .FilterSummary<SummaryIdQuery, ActivityEntity>(orConcatenation, filters, sorting)
               .AsNoTracking()
               .ToDictionaryAsync(k => k.Id, v => v.Name);
        }

        public async Task<Activity> FetchActivity(Guid id)
        {
            ActivityEntity entity = await _speurzoekersDbContext.Activities.FindAsync(id);
            Activity act = null;

            if (entity != null)
            {
                act = _mapper.Map<Activity>(entity);
            }

            return act;
        }

        public async Task AddActivity(Activity Activity)
        {
            ActivityEntity entity = _mapper.Map<ActivityEntity>(Activity);
            await _speurzoekersDbContext.Activities.AddAsync(entity);
            await _speurzoekersDbContext.SaveChangesAsync();
        }

        public async Task AddOrUpdateActivity(Activity Activity)
        {
            if (Activity.Id == Guid.Empty)
            {
                ActivityEntity reg = _mapper.Map<ActivityEntity>(Activity);
                await _speurzoekersDbContext.Activities.AddAsync(reg);
                _mapper.Map(reg, Activity); // remap ID
            }
            else
            {
                ActivityEntity entity = await _speurzoekersDbContext.Activities.FindAsync(Activity.Id);
                _speurzoekersDbContext.Activities.Attach(entity).CurrentValues.SetValues(Activity);
            }

            await _speurzoekersDbContext.SaveChangesAsync();
        }

        public async Task RemoveActivity(Guid id)
        {
            ActivityEntity entity = _speurzoekersDbContext.Activities.Find(id);

            if (entity != null)
            {
                _speurzoekersDbContext.Activities.Remove(entity);

                await _speurzoekersDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> TagNameExistOutsideId(Guid id, string tagName)
        {
            return await _speurzoekersDbContext.Activities
                .AnyAsync(x => x.Id != id && 
                    (string.Equals(x.TagName1, tagName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(x.TagName2, tagName, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<bool> NameExistOutsideId(Guid id, string name)
        {
            return await _speurzoekersDbContext.Activities
                .AnyAsync(x => x.Id != id && string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<string> GetTagName(Guid id, int index)
        {
            ActivityEntity entity = await _speurzoekersDbContext.Activities.FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            /* else */ if (index == 0)
            {
                return entity.TagName1;
            }
            // else
            return entity.TagName2;
        }
    }
}
