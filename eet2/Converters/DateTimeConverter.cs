using System;

namespace Mews.Eet.Converters
{
    public static class DateTimeConverter
    {
        public static DateTime ToEetDateTime(DateTimeWithTimeZone dateTimeWithTimeZone)
        {
            var dateTimeUtc = TimeZoneInfo.ConvertTime(dateTimeWithTimeZone.DateTime, dateTimeWithTimeZone.TimeZoneInfo, TimeZoneInfo.Utc);
            var dateTimeCz = TimeZoneInfo.ConvertTime(dateTimeUtc, TimeZoneInfo.Utc, TimeZoneInfo.Local);
            return DateTime.SpecifyKind(new DateTime(dateTimeCz.Ticks - dateTimeCz.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Local);
        }
    }
}
