using Speurzoekers.Data.Entities.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Speurzoekers.Data.Entities.Avatar
{
    public class AvatarTypeEntity
    {
        [Key]
        public long Index { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string TagName { get; set; }

        public int Points { get; set; }


        public virtual ICollection<AvatarTypeTextEntity> AvatarTypeTexts { get; set; }

        public virtual ICollection<UserInfoEntity> Users { get; set; }
    }
}
