using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Collectible;
using Speurzoekers.Common.Domain.CollectibleType;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Data.Entities;
using Speurzoekers.Data.Entities.Collectible;
using Speurzoekers.Data.Entities.Queries;
using Speurzoekers.Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Speurzoekers.Data.Repositories
{
    public class CollectibleRepository : ICollectibleRepository
    {
        private readonly SpeurzoekersDbContext _speurzoekersDbContext;
        private readonly IMapper _mapper;
        private readonly IConfigurationRepository _configurationRepository;

        public CollectibleRepository(
            SpeurzoekersDbContext speurzoekersDbContext, 
            IMapper mapper, 
            IConfigurationRepository configurationRepository)
        {
            _speurzoekersDbContext = speurzoekersDbContext;
            _mapper = mapper;
            _configurationRepository = configurationRepository;
        }

        public async Task ReplaceAllCollectibles(List<CollectiblePosition> collectibles)
        {
            List<CollectiblePositionEntity> collectiblePositionList =
            _mapper.Map<List<CollectiblePosition>, List<CollectiblePositionEntity>>(collectibles);

            await _speurzoekersDbContext.CollectiblePositions.RemoveAll();
            await _speurzoekersDbContext.CollectiblePositions.AddRangeAsync(collectiblePositionList);

            await _speurzoekersDbContext.SaveChangesAsync();
        }

        public async Task<string> GetTagName(Guid collectibleId)
        {
            var collectibleTypeEntity = await _speurzoekersDbContext.CollectibleTypes.FindAsync(collectibleId);
            return collectibleTypeEntity?.TagName;
        }

        public async Task<Guid[]> GetCollectibleTypeIds()
        {
            return await _speurzoekersDbContext.CollectibleTypes
                .Select(x => x.Id)
                .ToArrayAsync();
        }

        public async Task<Guid[]> GetCollectibleTypeIdsAtBiotope(Biotope biotope)
        {
            return await _speurzoekersDbContext.CollectibleTypes
                .Where(x => x.Biotopes.HasFlag(biotope))
                .Select(x => x.Id)
                .ToArrayAsync();
        }

        public async Task<CollectibleTypeSummary[]> GetCollectibleTypeSummaries(CalenderMonth month)
        {
            return await _speurzoekersDbContext.CollectibleTypes
                .Where(x => x.MonthsAvailable.HasFlag(month))
                .ProjectTo<CollectibleTypeSummary>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
  
        public async Task<List<CollectiblePosition>> GetCollectiblePositions(float latitude, float longitude)
        {
            float range = await _configurationRepository.GetSpottingRange();
            return await _speurzoekersDbContext.CollectiblePositions.
                Where(p => Math.Abs(p.Latitude - latitude) < range && Math.Abs(p.Longitude - longitude) < range)
                .ProjectTo<CollectiblePosition>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task SaveCollectibles(IEnumerable<CollectibleType> collectibleTypes, bool erase)
        {
            var list = _mapper.Map<IEnumerable<CollectibleType>, IEnumerable<CollectibleTypeEntity>>(collectibleTypes);

            if (erase)
            {
                IEnumerable<string> names = list.Select(x => x.Name);
                await _speurzoekersDbContext.CollectibleTypes.RemoveWhereIn(x => x.Name, names);
            }

            await _speurzoekersDbContext.CollectibleTypes.AddRangeAsync(list);
            await _speurzoekersDbContext.SaveChangesAsync();
        }

        public async Task AddOrUpdateCollectible(CollectibleType collectibleType)
        {
            if (collectibleType.Id == Guid.Empty)
            {
                CollectibleTypeEntity col = _mapper.Map<CollectibleTypeEntity>(collectibleType);
                await _speurzoekersDbContext.CollectibleTypes.AddAsync(col);
                _mapper.Map(col, collectibleType); // remap ID's
            }
            else
            {
                await _speurzoekersDbContext.CollectibleTypes.Update(collectibleType);
                await _speurzoekersDbContext.CollectibleTypeTexts.UpsertRange(
                    collectibleType.CollectibleTypeTexts.ToList(), x => x.CollectibleTypeId, 
                    collectibleType.Id, Guid.NewGuid, Guid.Empty);
                await _speurzoekersDbContext.CollectibleTypeChoices.UpsertRange(
                    collectibleType.CollectibleTypeChoices.ToList(), x => x.CollectibleTypeId, 
                    collectibleType.Id, Guid.NewGuid, Guid.Empty);
            }

            await _speurzoekersDbContext.SaveChangesAsync();  
        }

        public async Task<List<string>> GetExistingNames(IEnumerable<string> names)
        {
            return await _speurzoekersDbContext.CollectibleTypes
                .Where(x => names.Contains(x.Name, StringComparer.OrdinalIgnoreCase))
                .Select(x => x.Name)
                .ToListAsync();
        }

        public async Task<bool> CollectiblePositionExists(Guid collectiblePositionId)
        {
            return (await _speurzoekersDbContext.CollectiblePositions.FindAsync(collectiblePositionId) != null);
        }

        public async Task<bool> CollectiblePositionAlreadyCaught(Guid collectiblePositionId, Guid userId)
        {
            return (await _speurzoekersDbContext.UserCollectibles.
                SingleOrDefaultAsync(c => c.CollectiblePositionId == collectiblePositionId
                && c.ApplicationUserId == userId) != null);
        }
               
        public async Task<UserCollectible> SaveCaughtCollectible(Guid collectiblePositionId, Guid userId)
        {
            CollectiblePositionEntity collectiblePosition = await _speurzoekersDbContext.CollectiblePositions
                                                                .Include(x => x.Region)
                                                                .Include(y => y.CollectibleType)
                                                                .SingleAsync(c => c.Id == collectiblePositionId);
            
            UserCollectibleEntity userCollectible = new UserCollectibleEntity
            {
                CollectiblePositionId = collectiblePositionId,
                CollectibleTypeId = collectiblePosition.CollectibleTypeId,                
                ApplicationUserId = userId,
                Biotope = collectiblePosition.Region.Biotope,
                CatchPoints = collectiblePosition.CollectibleType.Points
            };
            await _speurzoekersDbContext.UserCollectibles.AddAsync(userCollectible);
            await _speurzoekersDbContext.SaveChangesAsync();
            return _mapper.Map<UserCollectible>(userCollectible);
        }

        public async Task<CollectibleType> GetCollectibleType(Guid collectibleTypeId)
        {
            CollectibleType collectibleType = await _speurzoekersDbContext.CollectibleTypes
                .Where(c => c.Id == collectibleTypeId)
                .Include(c => c.CollectibleTypeTexts)
                .Include(c => c.CollectibleTypeChoices)
                .ProjectTo<CollectibleType>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return collectibleType;
        }

        public async Task<CollectibleTypeNameAndPoints> GetCollectibleTypeNameAndPoints(Guid collectibleTypeId)
        {
            CollectibleTypeNameAndPoints collectibleTypeNameAndPoints = await _speurzoekersDbContext.CollectibleTypes
                .Where(c => c.Id == collectibleTypeId)
                .Select(c => new CollectibleTypeNameAndPoints {Name = c.Name, Points = c.Points }).FirstOrDefaultAsync();

            return collectibleTypeNameAndPoints;
        }

        public async Task<CollectibleType> FetchCollectibleType(Guid collectibleTypeId)
        {
            CollectibleTypeEntity entity = await _speurzoekersDbContext.CollectibleTypes.FindAsync(collectibleTypeId);
            CollectibleType col = null;

            if (entity != null)
            {
                await _speurzoekersDbContext.Entry(entity).Collection(i => i.CollectibleTypeTexts).LoadAsync();
                await _speurzoekersDbContext.Entry(entity).Collection(i => i.CollectibleTypeChoices).LoadAsync();
                col = _mapper.Map<CollectibleType>(entity);
            }

            return col;
        }

        public async Task<Dictionary<Guid, string>> GetCollectibleTypeSummary()
        {
            return await _speurzoekersDbContext.CollectibleTypes
                .AsNoTracking()
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToDictionaryAsync(k => k.Id, v => v.Name);
        }

        public Task<Dictionary<Guid, string>> GetCollectibleTypeSummary(
            IEnumerable<FilterCriterion> filters, bool orConcatenation, Sorting sorting)
        {
             return _speurzoekersDbContext.GuidSummary
                .FilterSummary<SummaryIdQuery, CollectibleTypeEntity>(orConcatenation, filters, sorting)
                .AsNoTracking()
                .ToDictionaryAsync(k => k.Id, v => v.Name);
        }

        public async Task RemoveCollectible(Guid id)
        {
            CollectibleTypeEntity entity = _speurzoekersDbContext.CollectibleTypes.Find(id);

            if (entity != null)
            {
                _speurzoekersDbContext.CollectibleTypes.Remove(entity);

                await _speurzoekersDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckQuestionAnswered(Guid userCollectibleId)
        {
            int? answered = await _speurzoekersDbContext.UserCollectibles
                .Where(c => c.Id == userCollectibleId)
                .Select(c => c.AnswerPoints).FirstAsync();
            if(answered != null)
            { return true; }
            else
            { return false; }
        }

        public async Task WriteQuestionAway(Guid userCollectibleId, bool answeredCorrect)
        {                        
            UserCollectibleEntity userCollectible = await _speurzoekersDbContext.UserCollectibles
                                                                .Include(y => y.CollectibleType)
                                                                .SingleAsync(c => c.Id == userCollectibleId);
            
            if (answeredCorrect)
            {
                userCollectible.AnswerPoints = userCollectible.CollectibleType.QuestionPoints;
            }
            else
            {
                userCollectible.AnswerPoints = 0;
            }
            await _speurzoekersDbContext.SaveChangesAsync();
        }

        public async Task<bool> NameExistOutsideId(Guid id, string name)
        {
            return await _speurzoekersDbContext.CollectibleTypes
                .AnyAsync(x => x.Id != id && string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> TagNameExistOutsideId(Guid id, string tagName)
        {
            return await _speurzoekersDbContext.CollectibleTypes
                .AnyAsync(x => x.Id != id && string.Equals(x.TagName, tagName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
