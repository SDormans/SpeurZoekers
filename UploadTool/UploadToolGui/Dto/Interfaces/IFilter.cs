namespace UploadToolGui.Dto.Interfaces
{
    public interface IFilter
    {
        FilterItem[] FilterList { get; }
        SortItem[] SortingList { get; }
    }
}
