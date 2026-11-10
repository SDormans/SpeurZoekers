using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Speurzoekers.Common.Domain.Avatar;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Data.Entities.Avatar;
using Speurzoekers.Data.Entities.Queries;
using Speurzoekers.Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Speurzoekers.Data.Repositories
{
    public class AvatarRepository : IAvatarRepository
    {
        private readonly SpeurzoekersDbContext _speurzoekersDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AvatarRepository(SpeurzoekersDbContext speurzoekersDbContext, IUserRepository userRepository, IMapper mapper)
        {
            _speurzoekersDbContext = speurzoekersDbContext;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<long?> GetAvatarIndex(Guid userId) {
            long? index = await _speurzoekersDbContext.UserInfos
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => x.ActiveAvatarIndex)
                .FirstOrDefaultAsync();
            return index;
        }
        public async Task<string> GetTagName(Guid userId)
        {
            string tagName = await _speurzoekersDbContext.UserInfos
                .Where(x => x.ApplicationUserId == userId)
                .Include(x => x.ActiveAvatar)
                .Select(x => x.ActiveAvatar.TagName)
                .FirstOrDefaultAsync();

            return tagName;
        }

        public async Task<string> GetTagName(long index)
        {
            AvatarTypeEntity entity = await _speurzoekersDbContext.AvatarTypes.FindAsync(index);
            return entity?.TagName;
        }

        public async Task<AvatarType> GetAvatarTypeInfo(long index)
        {
            AvatarType avatarTypeInfo = await _speurzoekersDbContext.AvatarTypes
                .Where(c => c.Index == index)
                .ProjectTo<AvatarType>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return avatarTypeInfo;
        }

        public async Task<List<AvatarTypeNoText>> GetAvatars()
        {
            return await _speurzoekersDbContext.AvatarTypes
                .ProjectTo<AvatarTypeNoText>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<AvatarTypeNoText>> GetAvatars(Guid userId)
        {
            IQueryable<long> collected = _speurzoekersDbContext.UserInfos
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => x.CollectedAvatars);

            return await _speurzoekersDbContext.AvatarTypes
                .Where(x => (x.Index & collected.First()) > 0)
                .ProjectTo<AvatarTypeNoText>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Dictionary<AvatarTypeNoText, bool>> GetAllAvatarsWithCollected(Guid userId)
        {
            IQueryable<long> collected = _speurzoekersDbContext.UserInfos
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => x.CollectedAvatars);

            return await _speurzoekersDbContext.AvatarTypes
                .ProjectTo<AvatarTypeNoText>(_mapper.ConfigurationProvider)
                .ToDictionaryAsync(k => k, v => (collected.First() & v.Index) > 0);
        }

        public Task AddAvatar(AvatarType avatar)
        {
            return AddAvatarType(avatar);
        }

        public async Task RemoveAvatar(long index)
        {
            AvatarTypeEntity entity = await _speurzoekersDbContext.AvatarTypes.FindAsync(index);
            _speurzoekersDbContext.AvatarTypes.Remove(entity);

            await _speurzoekersDbContext.UserInfos.UpdateWithExpression(x => x.CollectedAvatars, $"&= ~{entity.Index}");

            await _speurzoekersDbContext.SaveChangesAsync();
        }

        public async Task SetUserAvatar(Guid userId, long index)
        {
            UserInfoEntity user = await _speurzoekersDbContext.UserInfos.FindAsync(userId);

            if ((user.CollectedAvatars & index) > 0) // Collected?
            {
                user.ActiveAvatarIndex = index;
                await _speurzoekersDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task SetFirstAvatar(Guid userId, long index)
        {
            UserInfoEntity user = await _speurzoekersDbContext.UserInfos.FindAsync(userId);

            if (user.CollectedAvatars > 0)
            {
                throw new ArgumentException();
            }

            user.CollectedAvatars = index;
            user.ActiveAvatarIndex = index;

            await _speurzoekersDbContext.SaveChangesAsync();
        }

        public async Task<bool> BuyAvatar(Guid userId, long index, bool setAsCurrent)
        {
            int? price = await _speurzoekersDbContext.AvatarTypes
                .Where(x => x.Index == index)
                .Select(x => (int?)x.Points)
                .FirstOrDefaultAsync();

            if (!price.HasValue)
            {
                throw new ArgumentException();
            }
            // else
            long points = await _userRepository.GetAvailablePoints(userId);
            UserInfoEntity user = await _speurzoekersDbContext.UserInfos.FindAsync(userId);
            bool isCollected = (user.CollectedAvatars & index) > 0;

            if (!isCollected && price > points)
            {
                return false;
            }
            else
            {
                if (!isCollected)
                {
                    user.CollectedAvatars |= index;
                    user.SpendPoints += price.Value;
                }

                if (setAsCurrent)
                {
                    user.ActiveAvatarIndex = index;
                }

                if (!isCollected || setAsCurrent)
                {
                    await _speurzoekersDbContext.SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task<AvatarType> FetchAvatar(long index)
        {
            AvatarTypeEntity entity = await _speurzoekersDbContext.AvatarTypes.FindAsync(index);
            AvatarType avatar = null;

            if (entity != null)
            {
                await _speurzoekersDbContext.Entry(entity).Collection(i => i.AvatarTypeTexts).LoadAsync();
                avatar = _mapper.Map<AvatarType>(entity);
            }

            return avatar;
        }

        public async Task<Dictionary<long, string>> GetAvatarSummary()
        {
            return await _speurzoekersDbContext.AvatarTypes
                .AsNoTracking()
                .Select(x => new { x.Index, x.Name })
                .OrderBy(x => x.Name)
                .ToDictionaryAsync(k => k.Index, v => v.Name);
        }

        public Task<Dictionary<long, string>> GetAvatarSummary(
            IEnumerable<FilterCriterion> filters, bool orConcatenation, Sorting sorting)
        {
            return _speurzoekersDbContext.IndexSummary
               .FilterSummary<SummaryIndexQuery, AvatarTypeEntity>(orConcatenation, filters, sorting)
               .AsNoTracking()
               .ToDictionaryAsync(k => k.Index, v => v.Name);
        }

        public async Task<bool> TagNameExistOutsideId(long index, string tagName)
        {
            return await _speurzoekersDbContext.AvatarTypes
                .AnyAsync(x => x.Index != index && string.Equals(x.TagName, tagName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task AddOrUpdateAvatar(AvatarType avatarType)
        {
            if (avatarType.Index == 0)
            {
                AvatarTypeEntity avatar = await AddAvatarType(avatarType);
                _mapper.Map(avatar, avatarType); // remap ID's
            }
            else
            {
                await _speurzoekersDbContext.AvatarTypes.Update(avatarType);
                await _speurzoekersDbContext.AvatarTypeTexts.UpsertRange(avatarType.AvatarTypeTexts.ToList(), 
                    x => x.AvatarTypeIndex, avatarType.Index, Guid.NewGuid, Guid.Empty);
            }

            await _speurzoekersDbContext.SaveChangesAsync();
        }

        public async Task<bool> NameExistOutsideId(long index, string name)
        {
            return await _speurzoekersDbContext.AvatarTypes
                .AnyAsync(x => x.Index != index && string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<AvatarTypeEntity> AddAvatarType(AvatarType avatar)
        {
            long index = await _speurzoekersDbContext.AvatarTypes.MaxAsync(x => x.Index) * 2;

            if (index > long.MaxValue)
            {
                throw new ArgumentOutOfRangeException("Maximum index value reached");
            }

            AvatarTypeEntity entity = _mapper.Map<AvatarTypeEntity>(avatar);
            entity.Index = index;

            await _speurzoekersDbContext.AvatarTypes.AddAsync(entity);

            return entity;
        }
    }
}
