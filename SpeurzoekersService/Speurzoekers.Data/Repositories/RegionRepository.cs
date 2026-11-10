using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Speurzoekers.Data.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly SpeurzoekersDbContext _speurzoekersDbContext;
        private readonly IMapper _mapper;

        public RegionRepository(SpeurzoekersDbContext speurzoekersDbContext, IMapper mapper)
        {
            _speurzoekersDbContext = speurzoekersDbContext;
            _mapper = mapper;
        }

        public async Task<Region[]> GetRegions()
        {
            var result = await _speurzoekersDbContext.Regions.ToArrayAsync();
            return _mapper.Map<Region[]>(result);
        }

        public async Task<List<Region>> GetRegionsHierarchy()
        {
            List<RegionEntity> list = await _speurzoekersDbContext.Regions
                .Include(x => x.Childeren)
                .ToListAsync();

            return _mapper.Map<List<Region>>(list);
        }

        public async Task<List<RegionSummary>> GetRegionsSummary()
        {
            return await _speurzoekersDbContext.Regions
                    .OrderBy(x => x.Name)
                    .ProjectTo<RegionSummary>(_mapper.ConfigurationProvider)
                    .ToListAsync();
        }

        public async Task<Region> GetRegion(string name)
        {
            Region region = await _speurzoekersDbContext.Regions
                .Where(c => c.Name == name)
                .ProjectTo<Region>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return region;
        }

        public async Task<Region> FetchRegion(Guid id)
        {
            RegionEntity entity = await _speurzoekersDbContext.Regions.FindAsync(id);
            Region reg = null;

            if (entity != null)
            {
                reg = _mapper.Map<Region>(entity);
            }

            return reg;
        }

        public async Task AddRegion(Region region)
        {
            RegionEntity entity = _mapper.Map<RegionEntity>(region);
            await _speurzoekersDbContext.Regions.AddAsync(entity);
            await _speurzoekersDbContext.SaveChangesAsync();
        }

        public async Task AddOrUpdateRegion(Region region)
        {
            if (region.Id == Guid.Empty)
            {
                RegionEntity reg = _mapper.Map<RegionEntity>(region);
                await _speurzoekersDbContext.Regions.AddAsync(reg);
                _mapper.Map(reg, region); // remap ID
            }
            else
            {
                RegionEntity entity = await _speurzoekersDbContext.Regions.FindAsync(region.Id);
                _speurzoekersDbContext.Regions.Attach(entity).CurrentValues.SetValues(region);
            }

            await _speurzoekersDbContext.SaveChangesAsync();
        }

        public async Task RemoveRegion(Guid id)
        {
            RegionEntity entity = _speurzoekersDbContext.Regions.Find(id);
            await RemoveRegion(entity);
        }

        public async Task RemoveRegion(string name)
        {
            RegionEntity entity = _speurzoekersDbContext.Regions.FirstOrDefault(x => x.Name == name);
            await RemoveRegion(entity);
        }

        private async Task RemoveRegion(RegionEntity entity)
        {
            if (entity != null)
            {
                await _speurzoekersDbContext.Regions
                    .Where(x => x.ParentId == entity.Id)
                    .ForEachAsync((r) => r.ParentId = null);

                _speurzoekersDbContext.Regions.Remove(entity);

                await _speurzoekersDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> NameExistOutsideId(Guid id, string name)
        {
            return await _speurzoekersDbContext.Regions
                .AnyAsync(x => x.Id != id && string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
