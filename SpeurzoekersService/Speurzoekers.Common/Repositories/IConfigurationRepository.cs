using System.Threading.Tasks;

namespace Speurzoekers.Common.Repositories
{
    public interface IConfigurationRepository
    {
        /// <summary>Gets the current catch range</summary>
        /// <returns>Current catch range</returns>
        Task<float> GetCatchRange();

        /// <summary>Sets the catch range, resets to default when you send null.</summary>
        /// <param name="range">New range or null</param>
        Task SetCatchRange(float? range);

        /// <summary>Gets the current spotting range</summary>
        /// <returns>Current spotting range</returns>
        Task<float> GetSpottingRange();

        /// <summary>Sets the spotting range, resets to default when you send null.</summary>
        /// <param name="range">New range or null</param>
        Task SetSpottingRange(float? range);

        /// <summary>Gets the map refresh rate in minutes</summary>
        /// <returns>Minutes</returns>
        Task<int> GetMapRefreshRate();

        /// <summary>Sets the map refresh rate in minutes</summary>
        /// <param name="minutes">Minutes</param>
        Task SetMapRefreshRate(int? minutes);

        /// <summary>Gets the map refresh time in minutes from 0:00, or null for immediately</summary>
        /// <returns>Minutes</returns>
        Task<int?> GetMapRefreshStart();

        /// <summary>Sets the map refresh time in minutes from 0:00, or null for immediately</summary>
        /// <param name="minutes">Minutes</param>
        Task SetMapRefreshStart(int? startTime);

        /// <summary>Get the default avatar price</summary>
        /// <returns>Points</returns>
        Task<int> GetAvatarPrice();

        /// <summary>Set the default avatar price, resets to default when you send null.</summary>
        /// <param name="points">Price in points</param>
        Task SetAvatarPrice(int? points);
    }
}
