using Microsoft.EntityFrameworkCore;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Speurzoekers.Data.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly SpeurzoekersDbContext _context;

        public ConfigurationRepository(SpeurzoekersDbContext context)
        {
            _context = context;
        }

        public async Task<float> GetCatchRange()
        {
            float? value = await GetConfiguration()
                .Select(x => (float?)x.CatchRange)
                .FirstOrDefaultAsync();

            return value ?? Configuration.CatchRange;
        }

        public async Task SetCatchRange(float? range)
        {
            await SetValue(range.HasValue, 
                c =>
                    {
                        c.CatchRange = range.Value;
                        return c;
                    }, 
                d =>
                    {
                        d.CatchRange = Configuration.CatchRange;
                        return d;
                    }
            );
        }

        public async Task<float> GetSpottingRange()
        {
            float? value = await GetConfiguration()
                .Select(x => (float?)x.SpottingRange)
                .FirstOrDefaultAsync();

            return value ?? Configuration.SpottingRange;
        }

        public async Task SetSpottingRange(float? range)
        {
            await SetValue(range.HasValue,
                c =>
                {
                    c.SpottingRange = range.Value;
                    return c;
                },
                d =>
                {
                    d.SpottingRange = Configuration.SpottingRange;
                    return d;
                }
            );
        }

        public async Task<int> GetMapRefreshRate()
        {
            int? value = await GetConfiguration()
                .Select(x => (int?)x.MapRefreshRate)
                .FirstOrDefaultAsync();

            return value ?? Configuration.MapRefreshRate;
        }

        public async Task SetMapRefreshRate(int? minutes)
        {
            await SetValue(minutes.HasValue,
                c =>
                {
                    c.MapRefreshRate = minutes.Value;
                    return c;
                },
                d =>
                {
                    d.MapRefreshRate = Configuration.MapRefreshRate;
                    return d;
                }
            );
        }

        public async Task<int?> GetMapRefreshStart()
        {
            return await GetConfiguration()
                .Select(x => x.MapRefreshStart)
                .FirstOrDefaultAsync();
        }

        public async Task SetMapRefreshStart(int? startTime)
        {
            await SetValue(true,
                c =>
                {
                    c.MapRefreshStart = startTime;
                    return c;
                },
                null
            );
        }

        public async Task<int> GetAvatarPrice()
        {
            int? value = await GetConfiguration()
                .Select(x => (int?)x.AvatarPrice)
                .FirstOrDefaultAsync();

            return value ?? Configuration.AvatarPrice;
        }

        public async Task SetAvatarPrice(int? points)
        {
            await SetValue(points.HasValue,
                c =>
                {
                    c.AvatarPrice = points.Value;
                    return c;
                },
                d =>
                {
                    d.AvatarPrice = Configuration.AvatarPrice;
                    return d;
                }
            );
        }

        private IQueryable<ConfigurationEntity> GetConfiguration()
        {
            return _context.Configuration.Take(1);
        }

        private async Task SetValue(
            bool hasValue,
            Func<ConfigurationEntity, ConfigurationEntity> setter,
            Func<ConfigurationEntity, ConfigurationEntity> defaultSetter)
        {
            ConfigurationEntity record = await _context.Configuration.FirstOrDefaultAsync();

            if (hasValue)
            {
                if (record != null)
                {
                    setter(record);
                }
                else
                {
                    _context.Configuration.Add(setter(GetDefault()));
                }
            }
            else if (record != null)
            {
                defaultSetter(record);
            }

            await _context.SaveChangesAsync();
        }

        private ConfigurationEntity GetDefault()
        {
            ConfigurationEntity output = new ConfigurationEntity();
            Configuration conf = new Configuration();

            var sourceProperties = conf.GetType().GetProperties();
            var destinationProperties = output.GetType().GetProperties()
                .Where(p => sourceProperties.Select(s => s.Name).Contains(p.Name));

            foreach (var dest in destinationProperties)
            {
                var source = sourceProperties.FirstOrDefault(p => p.Name == dest.Name);

                if (source.GetValue(conf) != null)
                {
                    dest.SetValue(output, Convert.ChangeType(source.GetValue(conf), dest.PropertyType));
                }
            }

            return output;
        }
    }
}
