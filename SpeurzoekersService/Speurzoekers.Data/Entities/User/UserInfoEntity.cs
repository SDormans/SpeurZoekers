using System;
using System.ComponentModel.DataAnnotations;
using Speurzoekers.Data.Entities.Avatar;
using Speurzoekers.Data.Entities.Identity;

namespace Speurzoekers.Data.Entities.User
{
    public class UserInfoEntity
    {
        [Key]
        public Guid ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { set; get; }

        public DateTime RegisterDate { get; set; }

        [MaxLength(256)]
        public string DisplayName { get; set; }

        public long? ActiveAvatarIndex { get; set; }
        public virtual AvatarTypeEntity ActiveAvatar { get; set; }
        public long CollectedAvatars { get; set; }

        public float Longitude { get; set; }
        public float Latitude { get; set; }

        public long SpendPoints { get; set; }

        public int TotalDistance { get; set; }
    }
}
