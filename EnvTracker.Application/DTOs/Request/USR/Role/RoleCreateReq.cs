using EnvTracker.Application.Common;

namespace EnvTracker.Application.DTOs.Request.USR.Role
{
    public class RoleCreateReq : BasePgDto
    {
        public required string role_name { get; set; }
        public int[]? permission_ids { get; set; }
    }
}
