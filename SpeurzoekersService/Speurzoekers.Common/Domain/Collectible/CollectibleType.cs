using Speurzoekers.Common.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Speurzoekers.Common.Domain.Collectible

{
    public class CollectibleType
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string TagName { get; set; }

        public CollectibleRarity Rarity { get; set; }

        public int Points { get; set; }

        public int QuestionPoints { get; set; }

        public string QuestionText { get; set; }

        public Biotope Biotopes { get; set; }

        public CalenderMonth MonthsAvailable { get; set; }

        public ICollection<CollectibleTypeText> CollectibleTypeTexts { get; set; }

        public ICollection<CollectibleTypeChoice> CollectibleTypeChoices { get; set; }
    }
}