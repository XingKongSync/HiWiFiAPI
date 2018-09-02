using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSpeedDemo
{
    class TimestampHelper
    {
        private static DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

        public static DateTime GetTime(long timestamp)
        {
            DateTime time = dtStart.AddSeconds(timestamp);
            return time;
        }
    }
}
