using Speurzoekers.Data.Entities.Collectible;
using System;

namespace Speurzoekers.Data.Entities
{
    public class CollectiblePositionEntity  
    {
        public Guid Id { get; set; }
        public Guid CollectibleTypeId { get; set; }
        public virtual CollectibleTypeEntity CollectibleType { get; set; }
        public float Longitude { get; set; }     
        public float Latitude { get; set; }
        public Guid RegionId { get; set; }
        public virtual RegionEntity Region { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
