using System;

namespace MewsEet
{
    public class DateTimeWithTimeZone
    {
        public DateTimeWithTimeZone(DateTime dateTime, TimeZoneInfo timezoneInfo)
        {
            DateTime = dateTime;
            TimeZoneInfo = timezoneInfo;
        }

        public DateTime EetDateTime
        {
            get
            {
                var dateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(DateTime, TimeZoneInfo);
                return TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, CzechTimeZone);
            }
        }

        public static DateTimeWithTimeZone CreateCzech(DateTime dateTime)
        {
            return new DateTimeWithTimeZone(dateTime, CzechTimeZone);
        }

        private DateTime DateTime { get; }

        private TimeZoneInfo TimeZoneInfo { get; }

        private static TimeZoneInfo CzechTimeZone
        {
            get { return TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"); }
        }
    }
}
