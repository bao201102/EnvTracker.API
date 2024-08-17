using EnvTracker.Application.Common;

namespace EnvTracker.Application.DTOs.Request.Public.ActivityLog
{
    public class ActivityLogCreateReq : BasePgDto
    {
        public int user_id { get; set; }
        public string context { get; set; }
        public string action { get; set; }
        public string message { get; set; }
    }
}
