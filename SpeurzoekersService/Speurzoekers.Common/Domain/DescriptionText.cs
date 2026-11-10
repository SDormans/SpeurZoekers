using Speurzoekers.Common.Domain.Enums;
using System;

namespace Speurzoekers.Common.Domain
{
    public class DescriptionText
    {
        public Guid Id { get; set; }

        public DescriptionPageHeader TextId { get; set; }

        public string Text { get; set; }
    }
}
