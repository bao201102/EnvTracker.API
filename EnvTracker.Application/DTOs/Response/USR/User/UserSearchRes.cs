namespace EnvTracker.Application.DTOs.Response.USR.User
{
    public class UserSearchRes
    {
        public int total_record { get; set; }
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string full_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }
        public bool is_approved { get; set; }
        public DateTime created_date { get; set; }
    }
}
