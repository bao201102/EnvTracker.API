using Dapper;
using EnvTracker.Application.Common;
using EnvTracker.Application.DTOs.Request.USR.Role;
using EnvTracker.Application.DTOs.Request.USR.User;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.USR.Role;
using EnvTracker.Application.DTOs.Response.USR.User;
using EnvTracker.Application.Services.Interfaces.USR;
using EnvTracker.Application.Utilities;
using EnvTracker.Domain.Interfaces;
using Npgsql;

namespace EnvTracker.Application.Services.Implements.USR
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IRepository repository)
            : base(repository)
        {
        }

        public async Task<CRUDResult<RoleRes>> ReadById(int roleId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_role_id", roleId);

                var result = await Repository.QueryFirstStoredProcPgSql<RoleRes>("usr.role_read_by_id", param, "p_result");

                if (result == null)
                {
                    return Error<RoleRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                return Success(result);
            }
            catch (NpgsqlException ex)
            {
                return Error<RoleRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<RoleRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message);
            }
        }

        public async Task<PagingResponse<RoleSearchRes>> Search(RoleSearchReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.QueryStoredProcPgSql<RoleSearchRes>("usr.role_search", param, "p_result");
                if (result == null || !result.Any())
                {
                    return new PagingResponse<RoleSearchRes>
                    {
                        StatusCode = CRUDStatusCodeRes.ResourceNotFound
                    };
                }

                return PagingSuccess(result, obj.page_index, obj.page_size, result.FirstOrDefault().total_record);
            }
            catch (NpgsqlException ex)
            {
                return PagingError<RoleSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return PagingError<RoleSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.GetExceptionMessage());
            }
        }

        public async Task<CRUDResult<bool>> Create(RoleCreateReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.ExecuteStoredProcPgSql("usr.role_create", param, "p_result");

                if (result < 1)
                {
                    return Error(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Lỗi cập nhật dữ liệu", data: false);
                }

                return Success(true);
            }
            catch (NpgsqlException ex)
            {
                return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage(), data: false);
            }
            catch (Exception ex)
            {
                return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message, data: false);
            }
        }

        public async Task<CRUDResult<bool>> Update(RoleUpdateReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.ExecuteStoredProcPgSql("usr.role_update", param, "p_result");

                if (result < 1)
                {
                    return Error(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Lỗi cập nhật dữ liệu", data: false);
                }

                return Success(true);
            }
            catch (NpgsqlException ex)
            {
                return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage(), data: false);
            }
            catch (Exception ex)
            {
                return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message, data: false);
            }
        }

        public async Task<CRUDResult<bool>> Delete(int roleId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_role_id", roleId);

                var result = await Repository.ExecuteStoredProcPgSql("usr.role_delete", param, "p_result");

                if (result < 1)
                {
                    return Error(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Lỗi cập nhật dữ liệu", data: false);
                }

                return Success(true);
            }
            catch (NpgsqlException ex)
            {
                return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage(), data: false);
            }
            catch (Exception ex)
            {
                return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message, data: false);
            }
        }
    }
}
