using UploadToolGui.Dto.Enums;
using Speurzoekers.Common.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UploadToolGui.Dto.Interfaces
{
    public interface IAppMode
    {
        ControlOption ControlOptions { get; }
        int NameMaxLength { get; }

        Task<ListItem[]> GetItems();
        Task<ListItem[]> GetItems(IEnumerable<FilterCriterion> filters, bool orConcatenation, Sorting sorting);
        IBase NewItem();
        Task<IBase> GetItem(object id);
        bool IsNewItem(object id);
    }
}
