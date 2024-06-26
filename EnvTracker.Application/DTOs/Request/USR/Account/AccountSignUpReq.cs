﻿namespace EnvTracker.Application.DTOs.Request.USR.Account
{
    public class AccountSignUpReq
    {
        public required string user_name { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public required string password { get; set; }
    }
}
