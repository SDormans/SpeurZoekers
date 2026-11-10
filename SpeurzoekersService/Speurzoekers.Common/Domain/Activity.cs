using Speurzoekers.Common.Domain.Enums;
using System;

namespace Speurzoekers.Common.Domain
{
    public class Activity
    {
        public const int NrOfPictures = 2;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string TagName1 { get; set; }

        public string TagName2 { get; set; }

        public string Text { get; set; }

        public Season Season { get; set; }
    }
}
