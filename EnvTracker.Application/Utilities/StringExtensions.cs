using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace EnvTracker.Application.Utilities
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static JObject ToSnakeCase(this JObject original)
        {
            var newObj = new JObject();

            foreach (var property in original.Properties())
            {
                var newPropertyName = property.Name.ToSnakeCase();
                newObj[newPropertyName] = property.Value;
            }

            return newObj;
        }

        public static bool IsValidEmail(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
