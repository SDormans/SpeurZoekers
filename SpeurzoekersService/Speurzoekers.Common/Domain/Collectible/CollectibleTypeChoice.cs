using System;

namespace Speurzoekers.Common.Domain.Collectible
{
    public class CollectibleTypeChoice
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public bool IsAnswer { get; set; }

        public Guid CollectibleTypeId { get; set; }
    }
}