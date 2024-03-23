using EnvTracker.Application.Common;

namespace EnvTracker.Application.DTOs.Request.USR.Role
{
    public class RoleUpdateReq : BasePgDto
    {
        public int role_id { get; set; }
        public required string role_name { get; set; }
        public int[]? permission_ids { get; set; }
    }
}
