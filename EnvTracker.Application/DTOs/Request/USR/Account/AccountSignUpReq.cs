namespace EnvTracker.Application.DTOs.Request.USR.Account
{
    public class AccountSignUpReq
    {
        public required string user_name { get; set; }
        public required string first_name { get; set; }
        public required string last_name { get; set; }
        public required string phone { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
    }
}
