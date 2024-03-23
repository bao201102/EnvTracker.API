namespace EnvTracker.Application.DTOs.Response.USR.Role
{
    public class RoleSearchRes
    {
        public int total_record { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }
        public int[] permission_ids { get; set; }
    }
}
