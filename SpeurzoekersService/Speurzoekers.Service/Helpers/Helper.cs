namespace Speurzoekers.Service.Helpers
{
    public static class Helper
    {
        public static string MinutesToString(int minutes)
        {
            const int day = 1440;

            if (minutes == 1)
            {
                return "1 minuut";
            }
            /*else*/
            if (minutes < 60)
            {
                return $"{minutes} minuten";
            }
            /*else*/
            if (minutes == day)
            {
                return "1 dag";
            }
            /*else*/
            if (minutes == day * 7)
            {
                return "1 week";
            }
            /*else*/
            if (minutes % (day * 7) == 0)
            {
                return $"{minutes / day / 7} weken";
            }
            /*else*/
            if (minutes < day * 2)
            {
                string result = $"{minutes / 60} uur";

                if (minutes % 60 == 1)
                {
                    result += $" en 1 minuut";
                }
                else if (minutes % 60 > 1)
                {
                    result += $" en {minutes % 60} minuten";
                }

                return result;
            }
            // else
            {
                return $"{minutes / day} dagen";
            }
        }
    }
}
