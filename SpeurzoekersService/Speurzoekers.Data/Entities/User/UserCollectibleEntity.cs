using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Data.Entities.Collectible;
using Speurzoekers.Data.Entities.Identity;
using System;

namespace Speurzoekers.Data.Entities.User
{
    public class UserCollectibleEntity
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public Guid CollectibleTypeId { get; set; }
        public virtual CollectibleTypeEntity CollectibleType { get; set; }
        public Guid CollectiblePositionId { get; set; }
        public int CatchPoints { get; set; }
        public int? AnswerPoints { get; set; }
        public Biotope Biotope { get; set; }
    }
}
