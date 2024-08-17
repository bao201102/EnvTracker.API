using EnvTracker.Application.DTOs.Response.USR.Role;

namespace EnvTracker.Application.DTOs.Response.USR.Account
{
    public class AccountSignInRes
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public IEnumerable<int> role_ids { get; set; }
        public IEnumerable<RoleRes> roles { get; set; }
    }
}
