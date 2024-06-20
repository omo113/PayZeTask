namespace PayZe.Shared
{
    public abstract class SystemDate
    {
        public static TimeZoneInfo GeoTimeZone => TimeZoneInfo.FindSystemTimeZoneById("Georgian Standard Time");

        public static DateTimeOffset Now
        {
            get
            {
                var date = DateTimeOffset.Now.ToUniversalTime();
                return date;
            }
        }
    }
}