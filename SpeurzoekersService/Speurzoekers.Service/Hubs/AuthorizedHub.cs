using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Speurzoekers.Service.Hubs
{
    [Authorize]
    public class AuthorizedHub : Hub
    { }

    [Authorize]
    public class AuthorizedHub<T> : Hub<T> where T : class
    { }
}
