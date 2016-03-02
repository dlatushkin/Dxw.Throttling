using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Throttling
{
    public enum TimeRange { Second, Minute, Hour, Day }

    public static class TimeRangeHelper
    {
        private static TimeSpan GetTimespan(this TimeRange range)
        {
            switch (range)
            {
                case TimeRange.Second: return TimeSpan.FromSeconds(1);
                case TimeRange.Minute: return TimeSpan.FromMinutes(1);
                case TimeRange.Hour: return TimeSpan.FromHours(1);
                case TimeRange.Day: return TimeSpan.FromDays(1);
                default: throw new ArgumentOutOfRangeException(nameof(range));
            }
        }
    }
}
