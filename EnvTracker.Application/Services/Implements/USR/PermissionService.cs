using EnvTracker.Application.Common;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.USR.Permission;
using EnvTracker.Application.Services.Interfaces.USR;
using EnvTracker.Application.Utilities;
using EnvTracker.Domain.Interfaces;
using Npgsql;

namespace EnvTracker.Application.Services.Implements.USR
{
    public class PermissionService : BaseService, IPermissionService
    {
        public PermissionService(IRepository repository)
            : base(repository)
        {
        }

        public async Task<CRUDResult<IEnumerable<PermissionRes>>> List()
        {
            try
            {
                var result = await Repository.QueryStoredProcPgSql<PermissionRes>("usr.permission_read_all", null, "p_result");

                if (result == null || !result.Any())
                {
                    return Error<IEnumerable<PermissionRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                return Success(result);
            }
            catch (NpgsqlException ex)
            {
                return Error<IEnumerable<PermissionRes>>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<IEnumerable<PermissionRes>>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message);
            }
        }

    }
}
