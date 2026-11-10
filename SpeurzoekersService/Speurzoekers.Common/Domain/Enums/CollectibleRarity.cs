using System.ComponentModel;

namespace Speurzoekers.Common.Domain.Enums
{
    public enum CollectibleRarity : byte
    {
        [Description("Zeer algemeen")]
        VeryCommon = 0,
        [Description("Algemeen")]
        Common = 1,
        [Description("Minder algemeen")]
        LessCommon = 2,
        [Description("Zeldzaam")]
        Rare = 3,
        [Description("Zeer zeldzaam")]
        VeryRare = 4
    }
}
