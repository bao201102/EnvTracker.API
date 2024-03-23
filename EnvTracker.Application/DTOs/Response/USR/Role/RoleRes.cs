namespace EnvTracker.Application.DTOs.Response.USR.Role
{
    public class RoleRes
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
        public int[] permission_ids { get; set; }
    }
}
