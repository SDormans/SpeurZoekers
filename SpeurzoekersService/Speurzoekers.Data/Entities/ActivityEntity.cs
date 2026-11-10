using Speurzoekers.Common.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Speurzoekers.Data.Entities
{
    public class ActivityEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(256)]
        public string TagName1 { get; set; }

        [MaxLength(256)]
        public string TagName2 { get; set; }

        public string Text { get; set; }

        public Season Season { get; set; }
    }
}
