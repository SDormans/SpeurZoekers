using System.Collections.Generic;

namespace Speurzoekers.Service.Dto.Avatar
{
    public class AvatarTypeDto
    {
        public ulong Index { get; set; }

        public string Name { get; set; }

        public int Points { get; set; }

        public ICollection<DescriptionTextDto> AvatarTypeTexts { get; set; }


    }
}
