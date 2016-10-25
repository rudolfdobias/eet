using System;

namespace Mews.Eet.Converters
{
    public static class DateTimeConverter
    {
        public static DateTime ToEetDateTime(DateTimeWithTimeZone dateTimeWithTimeZone)
        {
            var dateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(dateTimeWithTimeZone.DateTime, dateTimeWithTimeZone.TimeZoneInfo);
            var czDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, DateTimeWithTimeZone.CzechTimeZone);
            return DateTime.SpecifyKind(new DateTime(czDateTime.Ticks - czDateTime.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Local);
        }
    }
}
