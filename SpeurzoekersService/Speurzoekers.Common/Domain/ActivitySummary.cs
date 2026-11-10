using Speurzoekers.Common.Domain.Enums;
using System;

namespace Speurzoekers.Common.Domain
{
    public class ActivitySummary
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Season Season { get; set; }

    }
}
