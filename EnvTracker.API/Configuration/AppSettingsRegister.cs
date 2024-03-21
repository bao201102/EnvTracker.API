using EnvTracker.Domain;
using EnvTracker.Infrastructure.Authentication;

namespace EnvTracker.API.Configuration
{
    public static class AppSettingsRegister
    {
        public static void SettingsBinding(this IConfiguration configuration)
        {
            do
            {
                AppConfig.ConnectionStrings = new ConnectionStrings();
                AppConfig.JwtSettings = new JwtSettings();
            }
            while (AppConfig.ConnectionStrings == null);

            configuration.Bind("ConnectionStrings", AppConfig.ConnectionStrings);
            configuration.Bind("JwtSettings", AppConfig.JwtSettings);
        }
    }
}
