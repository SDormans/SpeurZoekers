using System;
using System.ComponentModel;

namespace Speurzoekers.Common.Helper
{
    public static class Extensions
    {
        /// <summary>Gets the DescriptionAttribute value</summary>
        public static string Description(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        /// <summary>Counts the number of set flags</summary>
        public static int FlagCount(this Enum val)
        {
            int count = 0;
            long e = Convert.ToInt64(val);

            while (e != 0)
            {
                e &= e - 1;

                count++;
            }

            return count;
        }
    }
}
