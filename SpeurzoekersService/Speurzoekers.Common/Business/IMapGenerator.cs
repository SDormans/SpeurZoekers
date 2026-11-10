using System;
using System.Threading.Tasks;

namespace Speurzoekers.Common.Business
{
    public interface IMapGenerator
    {
        /// <summary>Replace all collectible positions</summary>
        Task RefreshMap(DateTime dateTime);
    }
}