using System;

namespace Speurzoekers.Common.Domain
{
    public class RegionSummary
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
