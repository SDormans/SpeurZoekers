using System;

namespace UploadToolGui.Dto.Enums
{
    [Flags]
    public enum ControlOption
    {
        LoadQuestion = 1 << 0,
        LoadAvailability = 1 << 1,
        LoadPicture = 1 << 2,
        MultipleTexts = 1 << 3,
        Rarity = 1 << 4,
        Points = 1 << 5,
        MultiplePicturesOptional = 1 << 6,
        SingleTagName = 1 << 7,
        Filter = 1 << 8
    }
}
