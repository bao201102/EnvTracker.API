using Dapper;
using EnvTracker.Domain.Interfaces;
using Npgsql;
using System.Data;
using System.Data.Common;
using static Dapper.SqlMapper;

namespace EnvTracker.Infrastructure.Data
{
    public class DapperRepository : IRepository
    {
        private readonly IDbConnection _Connection;

        private bool disposedValue = false;

        public IDbConnection Connection => _Connection;

        public DapperRepository(IDbConnection Connection)
        {
            _Connection = Connection;
        }

        public async Task Reconnect()
        {
            if (_Connection is NpgsqlConnection npgsqlConnection)
            {
                if (npgsqlConnection.State == ConnectionState.Closed || _Connection.State == ConnectionState.Broken)
                {
                    await npgsqlConnection.OpenAsync();
                }
            }
        }

        private string ToPostgresStoredStatement(string procedureName, DynamicParameters param, string[] resultParams)
        {
            string empty = string.Empty;
            if (param != null)
            {
                IEnumerable<string> values = param.ParameterNames.Select((x) => "@" + x);
                empty = "(" + string.Join(",", values) + ")";
            }
            else
            {
                empty = "()";
            }

            string fetchQuery = string.Empty;
            if (resultParams?.Any() ?? false)
            {
                resultParams.ToList().ForEach(delegate (string x)
                {
                    fetchQuery = fetchQuery + " FETCH ALL IN " + x + ";";
                });
            }

            return "CALL " + procedureName + empty + "; " + fetchQuery;
        }

        public async Task<IEnumerable<T>> QueryStoredProcPgSql<T>(string procName, DynamicParameters parameters, string resultParam, IDbTransaction? tran = null)
        {
            await Reconnect();
            IDbTransaction transaction = tran ?? _Connection.BeginTransaction();
            try
            {
                string query = ToPostgresStoredStatement(procName, parameters, new string[1] { resultParam });
                GridReader multi = await _Connection.QueryMultipleAsync(query, parameters,
                                                                                    commandType: CommandType.Text,
                                                                                    transaction: transaction,
                                                                                    commandTimeout: 300);
                await multi.ReadAsync<object>();
                IEnumerable<T> result = await multi.ReadAsync<T>();
                if (tran == null)
                {
                    transaction.Commit();
                }

                return result;
            }
            catch (NpgsqlException ex)
            {
                if (ex.Message.Contains("terminating connection due to administrator command"))
                {
                    return await QueryStoredProcPgSql<T>(procName, parameters, resultParam, tran);
                }

                transaction?.Rollback();
                throw ex;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<T?> QueryFirstStoredProcPgSql<T>(string procName, DynamicParameters parameters, string resultParam, IDbTransaction? tran = null)
        {
            await Reconnect();
            IDbTransaction transaction = tran ?? _Connection.BeginTransaction();
            try
            {
                string query = ToPostgresStoredStatement(procName, parameters, new string[1] { resultParam });
                GridReader multi = await _Connection.QueryMultipleAsync(query, parameters,
                                                                commandType: CommandType.Text,
                                                                transaction: transaction,
                                                                commandTimeout: 300);
                await multi.ReadAsync<object>();
                IEnumerable<T> result = await multi.ReadAsync<T>();
                if (tran == null)
                {
                    transaction.Commit();
                }

                return result.FirstOrDefault();
            }
            catch (NpgsqlException ex)
            {
                if (ex.Message.Contains("terminating connection due to administrator command"))
                {
                    return await QueryFirstStoredProcPgSql<T>(procName, parameters, resultParam, tran);
                }

                transaction?.Rollback();
                throw ex;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<GridReader> QueryMultiStoredProcPgSql(string procName, DynamicParameters parameters, params string[] resultParams)
        {
            await Reconnect();
            try
            {
                string query = ToPostgresStoredStatement(procName, parameters, resultParams);
                GridReader result = await _Connection.QueryMultipleAsync(query, parameters, null, null, CommandType.Text);

                await result.ReadAsync<object>();

                return result;
            }
            catch (NpgsqlException ex)
            {
                if (ex.Message.Contains("terminating connection due to administrator command"))
                {
                    return await QueryMultiStoredProcPgSql(procName, parameters, resultParams);
                }

                throw ex;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> ExecuteStoredProcPgSql(string procName, DynamicParameters parameters, string resultParam, IDbTransaction? tran = null)
        {
            await Reconnect();
            IDbTransaction transaction = tran ?? _Connection.BeginTransaction();
            try
            {
                parameters.Add(resultParam, 0);
                string query = ToPostgresStoredStatement(procName, parameters, null);

                var gridReader = await _Connection.QueryMultipleAsync(query, parameters, transaction, null, CommandType.Text);
                IEnumerable<int> result = await gridReader.ReadAsync<int>();

                if (tran == null)
                {
                    transaction.Commit();
                }

                return result.First();
            }
            catch (NpgsqlException ex)
            {
                if (ex.Message.Contains("terminating connection due to administrator command"))
                {
                    return await ExecuteStoredProcPgSql(procName, parameters, resultParam, tran);
                }

                transaction?.Rollback();
                throw ex;
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
        }

        public async Task<T> ExecuteStoredProcPgSql<T>(string procName, DynamicParameters parameters, string resultParam, IDbTransaction? tran = null)
        {
            await Reconnect();
            IDbTransaction transaction = tran ?? _Connection.BeginTransaction();
            try
            {
                parameters.Add(resultParam, default(T));

                string query = ToPostgresStoredStatement(procName, parameters, null);
                var gridReader = await _Connection.QueryMultipleAsync(query, parameters, transaction, null, CommandType.Text);
                IEnumerable<T> result = await gridReader.ReadAsync<T>();

                if (tran == null)
                {
                    transaction.Commit();
                }

                return result.First();
            }
            catch (NpgsqlException ex)
            {
                if (ex.Message.Contains("terminating connection due to administrator command"))
                {
                    return await ExecuteStoredProcPgSql<T>(procName, parameters, resultParam, tran);
                }

                transaction?.Rollback();
                throw ex;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing && _Connection != null)
            {
                if (_Connection.State != 0)
                {
                    _Connection.Close();
                }

                _Connection.Dispose();
            }

            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~DapperRepository()
        {
            Dispose(disposing: false);
        }
    }
}
