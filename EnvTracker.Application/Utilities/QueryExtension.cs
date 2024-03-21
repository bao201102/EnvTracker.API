using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Web;

namespace EnvTracker.Application.Utilities
{
    public static class QueryExtension
    {
        public static string ConvertNpgsqlExceptionMessage(this string message)
        {
            if (message.Contains(":"))
            {
                var arrMessages = message.Split(':');
                return message.Substring(arrMessages[0].Length + 1, message.Length - arrMessages[0].Length - 1);
            }

            return message;
        }
    }
}
