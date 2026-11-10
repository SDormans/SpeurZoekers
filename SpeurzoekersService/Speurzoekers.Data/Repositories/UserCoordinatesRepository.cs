using Microsoft.EntityFrameworkCore;
using Speurzoekers.Common.Repositories;
using System;
using System.Threading.Tasks;

namespace Speurzoekers.Data.Repositories
{
    public class UserCoordinatesRepository : IUserCoordinatesRepository
    {
        private readonly SpeurzoekersDbContext _speurzoekersDbContext;

        public UserCoordinatesRepository(SpeurzoekersDbContext speurzoekersDbContext)
        {
            _speurzoekersDbContext = speurzoekersDbContext;
        }

        public async Task SaveCoordinatesAsync(Guid userId, float longitude, float latitude)
        {
            var userInfo = await _speurzoekersDbContext.UserInfos.SingleOrDefaultAsync(u => u.ApplicationUserId == userId);
            userInfo.Longitude = longitude;
            userInfo.Latitude = latitude;
            await _speurzoekersDbContext.SaveChangesAsync();
        }
    }
}
