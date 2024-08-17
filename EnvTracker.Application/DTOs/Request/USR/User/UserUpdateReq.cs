﻿namespace EnvTracker.Application.DTOs.Request.USR.User
{
    public class UserUpdateReq
    {
        public int user_id { get; set; }
        public required string user_name { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public required string password { get; set; }
        public bool is_approved { get; set; }
        public IEnumerable<int> role_ids { get; set; }
    }
}
