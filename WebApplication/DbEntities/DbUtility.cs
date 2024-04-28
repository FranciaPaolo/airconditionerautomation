using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeDev.DbEntities
{
    public class DbUtility
    {
        public static string GetStrDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string GetStrDateTime(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm");
        }

        public static string GetStrDateTime_floor5Minutes(DateTime date)
        {
            // arrotonda al multiplo dei 5 minuti inferiore
            DateTime floor = GetDateTime_floor5Minutes(date);
            return GetStrDate(floor);
        }

        public static DateTime GetDateTime_floor5Minutes(DateTime date)
        {
            // arrotonda al multiplo dei 5 minuti inferiore
            int minutes = Convert.ToInt32(Math.Floor(date.Minute / 5.0) * 5.0);
            DateTime floor = new DateTime(date.Year, date.Month, date.Day, date.Hour, minutes, 0);
            return floor;
        }

    }
}