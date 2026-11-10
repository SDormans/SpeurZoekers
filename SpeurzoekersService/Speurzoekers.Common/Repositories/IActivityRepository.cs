using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speurzoekers.Common.Repositories
{
    public interface IActivityRepository
    {
        /// <summary>Get all activities</summary>
        /// <returns>List of activities</returns>
        Task<Activity[]> GetActivities();

        /// <summary>Get all activities for the given season(s)</summary>
        /// <param name="season">Season(s)</param>
        /// <returns>List of activities</returns>
        Task<Activity[]> GetActivities(Season season);

        /// <summary>Get name and season of all activities</summary>
        /// <returns>List of names and seasons of activities</returns>
        Task<ActivitySummary[]> GetActivitiesNameAndSeason();

        /// <summary>Get a summary of all activities sorted on name</summary>
        /// <returns> List of activity IDs and names</returns>
        Task<Dictionary<Guid, string>> GetActivitiesSummary();

        /// <summary>Get a summary of all activities for the given season(s), sorted on name</summary>
        /// <param name="season">Season(s)</param>
        /// <returns> List of activity IDs and names</returns>
        Task<Dictionary<Guid, string>> GetActivitiesSummary(Season season);

        /// <summary>Get a summary of filtered activities sorted on name</summary>
        /// <param name="criteria">List of filter criteria</param>
        /// <param name="orConcatenation">Join criteria by OR, otherwise AND</param>
        /// <param name="sorting">Sorting</param>
        /// <returns> List of activities IDs and names</returns>
        Task<Dictionary<Guid, string>> GetActivitiesSummary(
            IEnumerable<FilterCriterion> criteria, bool orConcatenation, Sorting sorting);

        ///<summary>(Re-)Attach activity</summary>
        /// <param name="id">Activity-ID</param>
        Task<Activity> FetchActivity(Guid id);

        /// <summary>Add an activity</summary>
        /// <param name="activity">Activity</param>
        Task AddActivity(Activity Activity);

        /// <summary>Add or update an activity</summary>
        /// <param name="activity">Activity</param>
        Task AddOrUpdateActivity(Activity Activity);

        /// <summary>Remove the given activity</summary>
        /// <param name="id">Activity-ID</param>
        Task RemoveActivity(Guid id);

        /// <summary>Check whether the given activity tag name exists outside the given ID</summary>
        /// <param name="id">Activity ID</param>
        /// <param name="tagName">File name</param>
        /// <returns>Name exists</returns>
        Task<bool> TagNameExistOutsideId(Guid id, string tagName);

        /// <summary>Check whether the given activity name exists outside the given ID</summary>
        /// <param name="id">Activity ID</param>
        /// <param name="name">Activity name</param>
        /// <returns>Name exists</returns>
        Task<bool> NameExistOutsideId(Guid id, string name);

        /// <summary>Get the activity tag name for the given ID and index</summary>
        /// <param name="id">Activity-ID</param>
        /// <param name="index">Picture index</param>
        /// <returns>Activity tag name(s)</returns>
        Task<string> GetTagName(Guid id, int index);
    }
}