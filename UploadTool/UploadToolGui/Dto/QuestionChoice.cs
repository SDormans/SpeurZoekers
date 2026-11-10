using System;

namespace Speurzoekers.Common.Domain.Collectible
{
    public class QuestionChoice
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public bool IsAnswer { get; set; }
    }
}