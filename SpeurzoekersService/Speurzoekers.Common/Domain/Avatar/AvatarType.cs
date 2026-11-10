using System.Collections.Generic;

namespace Speurzoekers.Common.Domain.Avatar
{
    public class AvatarType
    {
        public long Index { get; set; }

        public string Name { get; set; }

        public string TagName { get; set; }

        public int Points { get; set; }

        public ICollection<AvatarTypeText> AvatarTypeTexts { get; set; }

    }
}
