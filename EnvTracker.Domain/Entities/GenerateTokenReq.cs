namespace EnvTracker.Domain.Entities
{
    public class GenerateTokenReq
    {
        public string user_name { get; set; }
        public string email { get; set; }
        public string full_name { get; set; }
        public string phone { get; set; }
        public int user_id { get; set; }
        public object permissions { get; set; }
    }
}
