using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UploadToolGui.Dto.Interfaces
{
    public interface IMainDto : IBase
    {
        CollectibleRarity Rarity { get; set; }
        int Points { get; set; }
        Season Season { get; set; }
        List<DescriptionText> Texts { get; set; }

        Task<bool> NameExistOutsideId();
    }

    public interface IMainDto<ID> : IMainDto, IBase<ID>
    { }
}
