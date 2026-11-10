using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Speurzoekers.Common.Domain;

namespace Speurzoekers.Common.Repositories
{
    public interface IRegionRepository
    {
        /// <summary>Get all regions</summary>
        /// <returns>Region array</returns>
        Task<Region[]> GetRegions();

        /// <summary>Get all regions hierarchical</summary>
        /// <returns>Region list</returns>
        Task<List<Region>> GetRegionsHierarchy();

        /// <summary>Get a summary of all regions sorted on name</summary>
        /// <returns> List of region IDs and names</returns>
        Task<List<RegionSummary>> GetRegionsSummary();

        /// <summary>Get the region</summary>
        Task<Region> GetRegion(string name);

        /// <summary>Fetch the region</summary>
        Task<Region> FetchRegion(Guid id);

        /// <summary>Add a region</summary>
        /// <param name="region">Region</param>
        Task AddRegion(Region region);

        /// <summary>Add or update a region</summary>
        /// <param name="collectible">Region</param>
        Task AddOrUpdateRegion(Region region);

        /// <summary>Remove thne given region</summary>
        Task RemoveRegion(Guid Id);

        /// <summary>Remove thne given region</summary>
        Task RemoveRegion(string name);

        /// <summary>Check whether the given region name exists outside the given ID</summary>
        /// <param name="id">Region ID</param>
        /// <param name="name">Region name</param>
        /// <returns>Name exists</returns>
        Task<bool> NameExistOutsideId(Guid id, string name);
    }
}
