using Speurzoekers.Common.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Speurzoekers.Data.Entities
{
    public class DescriptionTextEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DescriptionPageHeader TextId { get; set; }
        public string Text { get; set; }
    }
}
