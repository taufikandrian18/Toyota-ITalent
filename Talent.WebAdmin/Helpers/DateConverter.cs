using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Helpers
{
    public static class DateConverter
    {
        public static DateTime ToIndonesiaTimeZone(this DateTime date)
        {
            TimeZoneInfo indonesiaTimeZoneId;
            try
            {
                // Try using timezone identifier on windows based system.
                indonesiaTimeZoneId = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                // Well, turns out its not Windows. So, we get unix' timezone identifier
                indonesiaTimeZoneId = TimeZoneInfo.FindSystemTimeZoneById("Asia/Jakarta");
            }
            var output = TimeZoneInfo.ConvertTimeFromUtc(date, indonesiaTimeZoneId);

            return output;
        }

        public static DateTime EndDayTime(this DateTime? date)
        {
            //var indonesiaTimeZoneId = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var output = date.GetValueOrDefault().AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);

            return output;
        }

        public static DateTime EndDayTime(this DateTime date)
        {
            //var indonesiaTimeZoneId = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var output = date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);

            return output;
        }
    }
}
