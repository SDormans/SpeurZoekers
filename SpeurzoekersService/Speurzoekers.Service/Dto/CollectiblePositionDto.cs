using System;

namespace Speurzoekers.Service.Dto
{
    public class CollectiblePositionDto
    {
        public Guid Id { get; set; }
        public Guid CollectibleTypeId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
