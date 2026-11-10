using Speurzoekers.Common.Domain.Collectible;
using System.Collections.Generic;

namespace UploadToolGui.Dto.Interfaces
{
    public interface IQuestionDto
    {
        int QuestionPoints { get; set; }
        string QuestionText { get; set; }
        ICollection<QuestionChoice> QuestionChoices { get; set; }
    }

    public interface IQuestionDto<ID> : IQuestionDto, IBase<ID>
    { }
}
