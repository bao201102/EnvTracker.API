namespace EnvTracker.Application.Utilities
{
    public static class DateTimeHelper
    {
        // Chuyển đổi từ DateTime sang Unix timestamp
        public static long DateTimeToUnixTimestamp(this DateTime dateTime)
        {
            var unixEpoch = DateTime.UnixEpoch;
            var timeSpan = dateTime - unixEpoch;
            return Convert.ToInt64(timeSpan.TotalSeconds);
        }

        // Chuyển đổi từ Unix timestamp sang DateTime
        public static DateTime UnixTimestampToDateTime(this long unixTimestamp)
        {
            var unixEpoch = DateTime.UnixEpoch;
            return unixEpoch.AddSeconds(unixTimestamp).ToLocalTime();
        }
    }
}
