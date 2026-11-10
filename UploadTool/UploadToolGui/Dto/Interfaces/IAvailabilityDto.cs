using System;
using System.Collections.Generic;

namespace UploadToolGui.Dto.Interfaces
{
    public interface IAvailabilityDto
    {
        Func<Enum>[] AvailabilityListProps { get; }

        void SetAvailabilityListProp(int index, IEnumerable<Enum> enums);
    }

    public interface IAvailabilityDto<ID> : IAvailabilityDto, IBase<ID>
    { }
}
