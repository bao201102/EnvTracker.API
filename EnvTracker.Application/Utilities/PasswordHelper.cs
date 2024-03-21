using System.Security.Cryptography;
using System.Text;

namespace EnvTracker.Application.Utilities
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            string key = "supper_secret_key_2011";
            string data = password;

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            string hashed = ComputeHMACSHA256(keyBytes, dataBytes);

            return hashed;
        }

        private static string ComputeHMACSHA256(byte[] keyBytes, byte[] dataBytes)
        {
            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
