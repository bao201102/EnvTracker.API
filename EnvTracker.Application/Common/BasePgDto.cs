using Dapper;
using EnvTracker.Application.Utilities;

namespace EnvTracker.Application.Common
{
    public class BasePgDto
    {
        public DynamicParameters ToDynamicParameters()
        {
            var parameter = new DynamicParameters();
            foreach (var prop in this.GetType().GetProperties())
            {
                parameter.Add($"p_{prop.Name.ToSnakeCase()}", prop.GetValue(this));
            }
            return parameter;
        }
    }
}
