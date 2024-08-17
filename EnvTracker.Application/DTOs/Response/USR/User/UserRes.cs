namespace EnvTracker.Application.DTOs.Response.USR.User
{
    public class UserRes
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public IEnumerable<int> role_ids { get; set; }
        public bool is_approved { get; set; }
    }
}
