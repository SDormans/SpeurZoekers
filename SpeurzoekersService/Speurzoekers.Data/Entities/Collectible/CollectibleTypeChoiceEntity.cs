using System;
using System.ComponentModel.DataAnnotations;

namespace Speurzoekers.Data.Entities.Collectible
{
    public class CollectibleTypeChoiceEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CollectibleTypeId { get; set; }

        public virtual CollectibleTypeEntity CollectibleType { get; set; }

        [StringLength(255)]
        public string Text { get; set; }

        public bool IsAnswer { get; set; }
    }
}