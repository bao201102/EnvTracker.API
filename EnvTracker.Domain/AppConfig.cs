namespace EnvTracker.Domain
{
    public class AppConfig
    {
        public static ConnectionStrings ConnectionStrings { get; set; }
        public static JwtSettings JwtSettings { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
    }
}
