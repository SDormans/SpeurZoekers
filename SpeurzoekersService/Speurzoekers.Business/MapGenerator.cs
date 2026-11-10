using Speurzoekers.Common.Business;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.CollectibleType;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Speurzoekers.Business
{
    public class MapGenerator : IMapGenerator
    {
        private const int MaxTries = 100;

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private readonly ICollectibleRepository _collectibleRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly Random _random;

        public MapGenerator(ICollectibleRepository collectibleRepository, IRegionRepository regionRepository)
        {
            _collectibleRepository = collectibleRepository;
            _regionRepository = regionRepository;
            _random = new Random();
        }

        public async Task RefreshMap(DateTime dateTime)
        {
            if (await _semaphore.WaitAsync(0)) // 0 = don't wait when blocked
            {
                List<Region> regions = await _regionRepository.GetRegionsHierarchy();
                Dictionary<Region, Counter> regionsCount = regions.ToDictionary(
                    k => k,
                    v => new Counter(_random.Next(v.MinPositions, v.MaxPositions + 1)));
                List<CollectiblePosition> positions = new List<CollectiblePosition>();
                CollectibleTypeSummary[] collectibles = await _collectibleRepository
                        .GetCollectibleTypeSummaries(dateTime.GetCalenderMonth());
                var biotopes = Enum.GetValues(typeof(Biotope)).Cast<Biotope>().ToArray();
                var rarities = Enum.GetValues(typeof(CollectibleRarity)).Cast<CollectibleRarity>().ToArray();

                var summaries = new Dictionary<Biotope, Dictionary<CollectibleRarity, CollectibleTypeSummary[]>>();
                foreach (var biotope in biotopes)
                {
                    var rarityCol = new Dictionary<CollectibleRarity, CollectibleTypeSummary[]>();

                    foreach (var rarity in rarities)
                    {
                        var colinbiorar = collectibles
                            .Where(c => c.Biotopes.HasFlag(biotope) && c.Rarity == rarity).ToArray();
                        rarityCol.Add(rarity, colinbiorar);
                    }
                    summaries.Add(biotope, rarityCol);
                }

                foreach (Region region in regions.Where(x => x.ParentId == null))
                {
                    positions.AddRange(CreateCollectiblesPerRegion(regionsCount, summaries, region));
                }

                await _collectibleRepository.ReplaceAllCollectibles(positions);

                _semaphore.Release();
            }
            else
            {
                throw new SynchronizationLockException();
            }
        }

        private List<CollectiblePosition> CreateCollectiblesPerRegion(
                    Dictionary<Region, Counter> regionsCount,
                    Dictionary<Biotope, Dictionary<CollectibleRarity, CollectibleTypeSummary[]>> summaries,
                    Region region)
        {
            Counter regionCount = regionsCount[region];
            List<CollectiblePosition> positions = new List<CollectiblePosition>();

            if (regionCount.Current < regionCount.Max)
            {
                float minLat = region.PolygonVertices.Min(v => v.Latitude),
                    minLon = region.PolygonVertices.Min(v => v.Longitude),
                    maxLat = region.PolygonVertices.Max(v => v.Latitude),
                    maxLon = region.PolygonVertices.Max(v => v.Longitude);

                for (int tries = 0; tries < MaxTries && regionCount.Current < regionCount.Max; tries++)
                {
                    float latitude = (float)(minLat + _random.NextDouble() * (maxLat - minLat)),
                            longitude = (float)(minLon + _random.NextDouble() * (maxLon - minLon));

                    if (InsideRegion(region, latitude, longitude))
                    {
                        PlacePosition(regionsCount, summaries, out CollectiblePosition pos, region, latitude, longitude);

                        if (pos != null)
                        {
                            positions.Add(pos);
                            tries = 0;
                        }
                    }

                }
            }

            foreach (Region child in region.Childeren)
            {
                positions.AddRange(CreateCollectiblesPerRegion(regionsCount, summaries, child));
            }

            return positions;
        }

        private bool PlacePosition(
            Dictionary<Region, Counter> regionsCount,
            Dictionary<Biotope, Dictionary<CollectibleRarity, CollectibleTypeSummary[]>> summaries,
            out CollectiblePosition position,
            Region region,
            float latitude,
            float longitude)
        {
            Counter regionCount = regionsCount[region];
            bool anyInside = false;
            position = null;

            for (int i = 0; !anyInside && i < region.Childeren.Count; i++)
            {
                Region child = region.Childeren[i];

                if (InsideRegion(child, latitude, longitude))
                {
                    anyInside = PlacePosition(regionsCount, summaries, out position, child, latitude, longitude);
                }
            }

            if (!anyInside)
            {
                anyInside = true;

                if (regionCount.Current < regionCount.Max)
                {
                    if (summaries[region.Biotope].Any(r => r.Value.Any()))
                    {
                        regionCount.Current++;

                        position = GenerateCollectible(region, summaries[region.Biotope], latitude, longitude);
                    }
                }
            }

            return anyInside;
        }

        private bool InsideRegion(Region region, float latitude, float longitude)
        {
            bool result = false;
            int j = region.PolygonVertices.Count() - 1;

            for (int i = 0; i < region.PolygonVertices.Count(); i++)
            {
                if (region.PolygonVertices[i].Longitude < longitude && region.PolygonVertices[j].Longitude >= longitude
                    || region.PolygonVertices[j].Longitude < longitude && region.PolygonVertices[i].Longitude >= longitude)
                {
                    if (region.PolygonVertices[i].Latitude + (longitude - region.PolygonVertices[i].Longitude)
                        / (region.PolygonVertices[j].Longitude - region.PolygonVertices[i].Longitude)
                        * (region.PolygonVertices[j].Latitude - region.PolygonVertices[i].Latitude) < latitude)
                    {
                        result = !result;
                    }
                }
                j = i;
            }

            return result;
        }

        private CollectiblePosition GenerateCollectible(Region region, Dictionary<CollectibleRarity, CollectibleTypeSummary[]> collectibleTypeSummaries, float lat, float lon)
        {
            TimeSpan? duration;

            CollectibleTypeSummary[] collection;
            do
            {
                int random = _random.Next(100);
                if (random < 30)
                {
                    collection = collectibleTypeSummaries[CollectibleRarity.VeryCommon];
                    duration = null;
                }
                else if (random < 55)
                {
                    collection = collectibleTypeSummaries[CollectibleRarity.Common];
                    duration = null;
                }
                else if (random < 75)
                {
                    collection = collectibleTypeSummaries[CollectibleRarity.LessCommon];
                    duration = new TimeSpan(2, 0, 0);
                }
                else if (random < 90)
                {
                    collection = collectibleTypeSummaries[CollectibleRarity.Rare];
                    duration = new TimeSpan(1, 0, 0);
                }
                else
                {
                    collection = collectibleTypeSummaries[CollectibleRarity.VeryRare];
                    duration = new TimeSpan(0, 30, 0);
                }
            } while (!collection.Any());


            Guid typeId = collection[_random.Next(collection.Length)].Id;

            DateTime? beginDateTime = duration.HasValue
                ? DateTime.Now.AddHours(_random.Next(24)).AddMinutes(_random.Next(60)).ToUniversalTime()
                : (DateTime?)null;

            return new CollectiblePosition
            {
                CollectibleTypeId = typeId,
                Latitude = lat,
                Longitude = lon,
                Region = region,
                BeginDate = beginDateTime,
                EndDate = beginDateTime?.Add(duration.Value)
            };
        }
    }
}
