using Speurzoekers.Common.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Speurzoekers.Data.Entities.Collectible
{
    public class CollectibleTypeEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string TagName { get; set; }

        public CollectibleRarity Rarity { get; set; }

        public int Points { get; set; }

        public int QuestionPoints { get; set; }

        [StringLength(255)]
        public string QuestionText { get; set; }

        public Biotope Biotopes { get; set; }

        public CalenderMonth MonthsAvailable { get; set; }


        public virtual ICollection<CollectibleTypeTextEntity> CollectibleTypeTexts { get; set; }

        public virtual ICollection<CollectibleTypeChoiceEntity> CollectibleTypeChoices { get; set; }
    }
}
