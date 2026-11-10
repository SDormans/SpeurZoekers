using System;
using System.Linq;

namespace Speurzoekers.Common.Domain.Enums
{
    public static class EnumExtensions
    {
        public static CalenderMonth GetCalenderMonth(this DateTime dateTime)
        {
            return (CalenderMonth)(1 << (dateTime.Month - 1));
        }

        public static CalenderMonth GetFirstMonth(this CalenderMonth month)
        {
            for (int i = (short)CalenderMonth.January; i <= (short)CalenderMonth.December; i *= 2)
            {
                if (((short)month & i) > 0)
                {
                    return (CalenderMonth)i;
                }
            }

            return 0;
        }

        public static CalenderMonth GetLastMonth(this CalenderMonth month)
        {
            for (int i = (short)CalenderMonth.December; i <= (short)CalenderMonth.January; i /= 2)
            {
                if (((short)month & i) > 0)
                {
                    return (CalenderMonth)i;
                }
            }

            return 0;
        }

        public static Season GetFirstSeason(this Season month)
        {
            for (int i = (byte)Season.Spring; i <= (byte)Season.Winter; i *= 2)
            {
                if (((byte)month & i) > 0)
                {
                    return (Season)i;
                }
            }

            return 0;
        }

        public static Season GetLastSeason(this Season month)
        {
            for (int i = (byte)Season.Winter; i <= (byte)Season.Spring; i /= 2)
            {
                if (((byte)month & i) > 0)
                {
                    return (Season)i;
                }
            }

            return 0;
        }

        public static CalenderMonth GetAllYear()
        {
            return (CalenderMonth)Enum.GetValues(typeof(CalenderMonth))
                .Cast<CalenderMonth>()
                .Select(x => (decimal)x)
                .Sum();
        }

        public static Season GetAllSeasons()
        {
            return (Season)Enum.GetValues(typeof(Season))
                .Cast<Season>()
                .Select(x => (decimal)x)
                .Sum();
        }

        public static Biotope GetAllBiotopes()
        {
            return (Biotope)Enum.GetValues(typeof(Biotope))
                .Cast<Biotope>()
                .Select(x => (decimal)x)
                .Sum();
        }
    }
}
