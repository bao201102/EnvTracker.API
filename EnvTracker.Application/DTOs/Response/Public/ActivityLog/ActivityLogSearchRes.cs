namespace EnvTracker.Application.DTOs.Response.Public.ActivityLog
{
    public class ActivityLogSearchRes
    {
        public int total_record { get; set; }
        public int activity_log_id { get; set; }
        public int user_id { get; set; }
        public string? user_name { get; set; }
        public string? context { get; set; }
        public string? action { get; set; }
        public string? message { get; set; }
    }
}
