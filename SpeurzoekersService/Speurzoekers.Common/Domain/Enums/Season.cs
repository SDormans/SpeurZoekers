using System;
using System.ComponentModel;

namespace Speurzoekers.Common.Domain.Enums
{
    [Flags]
    public enum Season : byte
    {
        [Description("Lente")]
        Spring = 1 << 0,
        [Description("Zomer")]
        Summer = 1 << 1,
        [Description("Herfst")]
        Autumn = 1 << 2,
        [Description("Winter")]
        Winter = 1 << 3
    }
}
