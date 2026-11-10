using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Collectible;
using Speurzoekers.Common.Domain.CollectibleType;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Common.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speurzoekers.Common.Repositories
{
    public interface ICollectibleRepository
    {
        /// <summary>Save list of collectibles</summary>
        /// <param name="collectibles">List of collectibles</param> 
        Task ReplaceAllCollectibles(List<CollectiblePosition> collectibles);

        /// <summary>Get the tag name for the given collectible</summary>
        /// <param name="collectibleId">Collectible-ID</param>
        /// <returns>Tag name</returns>
        Task<string> GetTagName(Guid collectibleId);
              
        /// <summary>Get all collectible type ID's</summary>
        /// <returns>ID's</returns>
        Task<Guid[]> GetCollectibleTypeIds();

        /// <summary>Get all collectible type ID's for a given biotope</summary>
        /// <param name="biotope">Biotope</param>
        /// <returns>Collectible type ID's</returns>
        Task<Guid[]> GetCollectibleTypeIdsAtBiotope(Biotope biotope);
        
        /// <summary>Get all collectible type ID/biotope/rarity for the given month</summary>
        /// <param name="month">Month</param>
        /// <returns>Collectible type ID/biotope</returns>
        Task<CollectibleTypeSummary[]> GetCollectibleTypeSummaries(CalenderMonth month);

        /// <summary>Get the current collectible postions</summary>
        /// <returns>List of collectible positions</returns>
        /// <param name="latitude">Latitude of user</param>
        /// <param name="longitude">Longitude of user</param>
        Task<List<CollectiblePosition>> GetCollectiblePositions(float latitude, float longitude);
        
        /// <summary>Add or update collectibles, based on the name</summary>
        /// <param name="collectibleTypes">List of collectible</param>
        /// <param name="erase">Erase existing entries</param>
        /// <returns>Generated IDs</returns>
        Task SaveCollectibles(IEnumerable<CollectibleType> collectibleTypes, bool erase);

        /// <summary>Add or update a collectible</summary>
        /// <param name="collectible">Collectible</param>
        Task AddOrUpdateCollectible(CollectibleType collectible);

        /// <summary>Check which of the given names are already in use.</summary>
        /// <param name="name">List of collectible names</param>
        /// <returns>Existing names</returns>
        Task<List<string>> GetExistingNames(IEnumerable<string> names);

        /// <summary>Checks if the CollectiblePosition exists in CollectiblePositions table</summary>
        /// <param name="collectiblePositionId">collectiblePositionId</param>
        /// <returns>True or false</returns>
        Task<bool> CollectiblePositionExists(Guid collectiblePositionId);

        /// <summary>Checks if the CollectiblePosition exists in UserCollectibles table</summary>
        /// <param name="collectiblePositionId">collectiblePositionId</param>
        /// <param name="userId">userId</param>
        /// <returns>True or false</returns>
        Task<bool> CollectiblePositionAlreadyCaught(Guid collectiblePositionId, Guid userId);

        /// <summary>Saves the caught CollectiblePosition in the UserCollectibles table</summary>
        /// <param name="collectiblePositionId">collectiblePositionId</param>
        /// <param name="userId">userId</param>
        /// <returns>The Id of the user collectible</returns>
        Task<UserCollectible> SaveCaughtCollectible(Guid collectiblePositionId, Guid userId);

        ///<summary>Get collectible type including subclasses</summary>
        /// <param name="collectibleTypeId">CollectibleType-ID</param>
        /// <returns>CollectibleType</returns>
        Task<CollectibleType> GetCollectibleType(Guid collectibleTypeId);

        ///<summary>Get name and points of collectible type</summary>
        /// <param name="collectibleTypeId">CollectibleType-ID</param>
        /// <returns>CollectibleTypeNameAndPoints</returns>
        Task<CollectibleTypeNameAndPoints> GetCollectibleTypeNameAndPoints(Guid collectibleTypeId);

        /// <summary>
        /// Check whether the question is already answered
        /// </summary>
        /// <param name="userCollectibleId"></param>
        /// <returns>QuestionAnswered</returns>
        Task<bool> CheckQuestionAnswered(Guid userCollectibleId);

        /// <summary>
        /// If question correct write collectibleType Questionpoints in AnswerPoints,
        /// If question incorrect write 0 in AnswerPoints
        /// </summary>
        /// <param name="userCollectibleId"></param>
        /// <param name="answeredCorrect"></param>
        /// <returns></returns>
        Task WriteQuestionAway(Guid userCollectibleId, Boolean answeredCorrect);

        ///<summary>(Re-)Attach collectible type including subclasses</summary>
        /// <param name="collectibleTypeId">CollectibleType-ID</param>
        /// <returns> CollectibleType</returns>
        Task<CollectibleType> FetchCollectibleType(Guid collectibleTypeId);

        /// <summary>Get a summary of all collectible types sorted on name</summary>
        /// <returns> List of collectible type IDs and names</returns>
        Task<Dictionary<Guid, string>> GetCollectibleTypeSummary();

        /// <summary>Get a summary of filtered collectible types sorted on name</summary>
        /// <param name="criteria">List of filter criteria</param>
        /// <param name="orConcatenation">Join criteria by OR, otherwise AND</param>
        /// <param name="sorting">Sorting</param>
        /// <returns> List of collectible type IDs and names</returns>
        Task<Dictionary<Guid, string>> GetCollectibleTypeSummary(
            IEnumerable<FilterCriterion> criteria, bool orConcatenation, Sorting sorting);

        /// <summary>Removes the given collectible</summary>
        /// <param name="id">Collectible ID</param>
        Task RemoveCollectible(Guid id);

        /// <summary>Check whether the given collectible name exists outside the given ID</summary>
        /// <param name="id">Collectible type ID</param>
        /// <param name="name">Collectible name</param>
        /// <returns>Name exists</returns>
        Task<bool> NameExistOutsideId(Guid id, string name);

        /// <summary>Check whether the given tag name exists outside the given ID</summary>
        /// <param name="id">Collectible type ID</param>
        /// <param name="tagName">File name</param>
        /// <returns>Tag name exists</returns>
        Task<bool> TagNameExistOutsideId(Guid id, string tagName);
    }
}
