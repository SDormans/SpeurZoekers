using System.Threading.Tasks;

namespace Speurzoekers.Service.Hubs.Clients
{
    public interface ICollectibleClient
    {
        Task RefreshMapAlert();
        Task CatchRangeToClient(float catchRange);
    }
}
