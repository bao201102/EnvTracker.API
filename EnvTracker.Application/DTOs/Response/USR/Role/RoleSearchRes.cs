using EnvTracker.Application.DTOs.Response.USR.Permission;

namespace EnvTracker.Application.DTOs.Response.USR.Role
{
    public class RoleSearchRes
    {
        public int total_record { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }
        public IEnumerable<int> permission_ids { get; set; }
        public IEnumerable<PermissionRes> permissions { get; set; }
    }
}
