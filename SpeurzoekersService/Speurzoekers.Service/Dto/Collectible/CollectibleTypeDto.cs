using System;
using System.Collections.Generic;

namespace Speurzoekers.Service.Dto.Collectible
{
    public class CollectibleTypeDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Points { get; set; }

        public int QuestionPoints { get; set; }

        public string QuestionText { get; set; }

        public ICollection<CollectibleTypeChoiceDto> CollectibleTypeChoices { get; set; }

        public ICollection<DescriptionTextDto> CollectibleTypeTexts { get; set; }
    }
}
