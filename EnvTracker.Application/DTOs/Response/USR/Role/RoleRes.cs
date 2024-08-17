using EnvTracker.Application.DTOs.Response.USR.Permission;

namespace EnvTracker.Application.DTOs.Response.USR.Role
{
    public class RoleRes
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
        public IEnumerable<int> permission_ids { get; set; }
        public IEnumerable<PermissionRes> permissions { get; set; }
    }
}
