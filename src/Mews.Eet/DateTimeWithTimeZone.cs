using System;

namespace Mews.Eet
{
    public class DateTimeWithTimeZone
    {
        public DateTimeWithTimeZone(DateTime dateTime, TimeZoneInfo timezoneInfo)
        {
            DateTime = dateTime;
            TimeZoneInfo = timezoneInfo;
        }

        public static TimeZoneInfo CzechTimeZone
        {
            get { return TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"); }
        }

        public DateTime DateTime { get; }

        public TimeZoneInfo TimeZoneInfo { get; }
    }
}