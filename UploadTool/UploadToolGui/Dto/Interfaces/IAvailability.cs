using Speurzoekers.Common.UploadTools;

namespace UploadToolGui.Dto.Interfaces
{
    public interface IAvailability
    {
        string[] AvailabilityListNames { get; }
        EnumDetail[][] AvailabilityLists { get; }
    }
}
