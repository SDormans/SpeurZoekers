using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Enums;

namespace Speurzoekers.Service.Dto
{
    public class RegionDto
    {
        public string Name { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public Biotope Biotope { get; set; }
        public Coordinate[] PolygonVertices { get; set; }
    }
}