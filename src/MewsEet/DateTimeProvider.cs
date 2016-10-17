using System;

namespace MewsEet
{
    public class DateTimeProvider
    {
        public static DateTimeWithTimeZone Now
        {
            get { return new DateTimeWithTimeZone(DateTime.UtcNow, TimeZoneInfo.Utc); }
        }
    }
}
