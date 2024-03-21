namespace EnvTracker.Application.DTOs.Response.USR.Account
{
    public class AccountRes
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string activation_code { get; set; }
        public int role_id { get; set; }
        public DateTime last_password_change_date { get; set; }
        public DateTime last_activity_date { get; set; }
        public DateTime last_login_date { get; set; }
        public DateTime last_lockout_date { get; set; }
        public bool is_locked_out { get; set; }
        public bool is_approved { get; set; }
        public string comment { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
    }
}
