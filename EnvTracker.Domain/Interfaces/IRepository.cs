using Dapper;
using System.Data;
using static Dapper.SqlMapper;

namespace EnvTracker.Domain.Interfaces
{
    public interface IRepository : IDisposable
    {
        IDbConnection Connection { get; }
        Task Reconnect();
        Task<IEnumerable<T>> QueryStoredProcPgSql<T>(
            string procName, DynamicParameters parameters, string resultParam, IDbTransaction? tran = null);
        Task<T?> QueryFirstStoredProcPgSql<T>(
            string procName, DynamicParameters parameters, string resultParam, IDbTransaction? tran = null);
        Task<GridReader> QueryMultiStoredProcPgSql(
            string procName, DynamicParameters parameters, params string[] resultParams);
        Task<int> ExecuteStoredProcPgSql(
            string procName, DynamicParameters parameters, string resultParam, IDbTransaction? tran = null);
        Task<T> ExecuteStoredProcPgSql<T>(
            string procName, DynamicParameters parameters, string resultParam, IDbTransaction? tran = null);
    }
}
