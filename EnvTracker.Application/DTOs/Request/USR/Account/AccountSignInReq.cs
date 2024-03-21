namespace EnvTracker.Application.DTOs.Request.USR.Account
{
    public class AccountSignInReq
    {
        public required string user_name { get; set; }
        public required string password { get; set; }
    }
}
