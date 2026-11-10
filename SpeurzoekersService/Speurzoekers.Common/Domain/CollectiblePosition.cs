using System;

namespace Speurzoekers.Common.Domain
{
    public class CollectiblePosition
    {
        public Guid Id { get; set; }
        public Guid CollectibleTypeId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public Region Region { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set;  }
    }
}
