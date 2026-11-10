using System;
using System.Threading.Tasks;

namespace Speurzoekers.Common.Repositories
{
    public interface IUserCoordinatesRepository
    {
        /// <summary>Update(/add) user coordinates</summary>
        /// <param name="userId">User ID</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="latitude">Latitude</param>
        Task SaveCoordinatesAsync(Guid userId, float longitude, float latitude);
    }
}
