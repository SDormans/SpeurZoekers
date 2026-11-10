using System;
using System.ComponentModel;

namespace Speurzoekers.Common.Domain.Enums
{
    [Flags]
    public enum Biotope : short
    {
        [Description("Stad")]
        Stad = 1 << 0,
        [Description("Park")]
        Park = 1 << 1,
        [Description("Bos")]
        Bos = 1 << 2,
        [Description("Weide")]
        Weide = 1 << 3,
        [Description("Water")]
        Water = 1 << 4,
        [Description("Zee/Strand")]
        ZeeStrand = 1 << 5,
        [Description("Duinen")]
        Duinen = 1 << 6,
        [Description("Heide")]
        Heide = 1 << 7,
        [Description("Tuin")]
        Tuin = 1 << 8,
        [Description("Speeltuin/Schoolplein")]
        SpeeltuinSchoolplein = 1 << 9,
        [Description("Rivieren")]
        Rivieren = 1 << 10,
        [Description("Moeras")]
        Moeras = 1 << 11,
        [Description("Zeeuwse Delta")]
        ZeeuwseDelta = 1 << 12,
        [Description("Wadden")]
        Wadden = 1 << 13
    }
}
