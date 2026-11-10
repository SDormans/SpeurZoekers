using System;

namespace Speurzoekers.Data.Entities.Collectible
{
    public class CollectibleTypeTextEntity : DescriptionTextEntity
    {
        public Guid CollectibleTypeId { get; set; }
        public virtual CollectibleTypeEntity CollectibleType { get; set; }
    }
}
