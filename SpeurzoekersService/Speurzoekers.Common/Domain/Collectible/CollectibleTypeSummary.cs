using System;
using Speurzoekers.Common.Domain.Enums;

namespace Speurzoekers.Common.Domain.CollectibleType
{
    public class CollectibleTypeSummary
    {
        public Guid Id { get; set; }
        public Biotope Biotopes { get; set; }
        public CollectibleRarity Rarity { get; set; }
    }
}
