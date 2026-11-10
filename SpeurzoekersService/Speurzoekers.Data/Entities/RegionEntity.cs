using Speurzoekers.Common.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Speurzoekers.Data.Entities
{
    public class RegionEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Polygon { get; set; }
        public Biotope Biotope { get; set; }
        public int MinPositions { get; set; }
        public int MaxPositions { get; set; }
        public Guid? ParentId { get; set; }

        public virtual RegionEntity Parent { get; set; }
        public virtual ICollection<RegionEntity> Childeren { get; set; }

        public virtual ICollection<CollectiblePositionEntity> CollectiblePositions { get; set; }
    }
}
