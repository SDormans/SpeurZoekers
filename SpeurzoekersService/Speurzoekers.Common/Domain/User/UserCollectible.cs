using System;
using Speurzoekers.Common.Domain.Enums;

namespace Speurzoekers.Common.Domain.User
{
    public class UserCollectible
    {
        public Guid Id { get; set; }
        public Guid CollectibleTypeId { get; set; }
        public Guid CollectiblePositionId { get; set; }
        public int CatchPoints { get; set; }
        public int? AnswerPoints { get; set; }
        public Biotope Biotope { get; set; }
    }
}
