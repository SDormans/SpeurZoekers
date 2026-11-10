using System;

namespace Speurzoekers.Data.Entities
{
    public class ConfigurationEntity
    {
        public Guid Id { get; set; }

        public float CatchRange { get; set; }
        public float SpottingRange { get; set; }

        public int MapRefreshRate { get; set; }
        public int? MapRefreshStart { get; set; }

        public int AvatarPrice { get; set; }
    }
}
