using System;

namespace Speurzoekers.Service.Dto
{
    public class AnswerQuestionDto
    {
        public Guid UserCollectibleId { get; set; }
        public bool AnsweredCorrect { get; set; }
    }
}
