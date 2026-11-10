using System;
using System.ComponentModel;

namespace Speurzoekers.Common.Domain.Enums
{
    [Flags]
    public enum CalenderMonth : short
    {
        [Description("Januari")]
        January = 1 << 0,
        [Description("Februari")]
        February = 1 << 1,
        [Description("Maart")]
        March = 1 << 2,
        [Description("April")]
        April = 1 << 3,
        [Description("Mei")]
        May = 1 << 4,
        [Description("Juni")]
        June = 1 << 5,
        [Description("Juli")]
        July = 1 << 6,
        [Description("Augustus")]
        August = 1 << 7,
        [Description("September")]
        September = 1 << 8,
        [Description("Oktober")]
        October = 1 << 9,
        [Description("November")]
        November = 1 << 10,
        [Description("December")]
        December = 1 << 11
    }
}
