using Dapper;
using EnvTracker.Application.Common;
using EnvTracker.Application.DTOs.Request.USR.User;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.USR.User;
using EnvTracker.Application.Services.Interfaces.USR;
using EnvTracker.Application.Utilities;
using EnvTracker.Domain.Interfaces;
using Npgsql;

namespace EnvTracker.Application.Services.Implements.USR
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IRepository repository)
            : base(repository)
        {
        }

        public async Task<CRUDResult<UserRes>> ReadById(int userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_user_id", userId);

                var result = await Repository.QueryFirstStoredProcPgSql<UserRes>("usr.user_read_by_id", param, "p_result");

                if (result == null)
                {
                    return Error<UserRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                return Success(result);
            }
            catch (NpgsqlException ex)
            {
                return Error<UserRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<UserRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message);
            }
        }

        public async Task<PagingResponse<UserSearchRes>> Search(UserSearchReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.QueryStoredProcPgSql<UserSearchRes>("usr.user_search", param, "p_result");
                if (result == null || !result.Any())
                {
                    return new PagingResponse<UserSearchRes>
                    {
                        StatusCode = CRUDStatusCodeRes.ResourceNotFound
                    };
                }

                return PagingSuccess(result, obj.page_index, obj.page_size, result.FirstOrDefault().total_record);
            }
            catch (NpgsqlException ex)
            {
                return PagingError<UserSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return PagingError<UserSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.GetExceptionMessage());
            }
        }

        public async Task<CRUDResult<bool>> Create(UserCreateReq obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj.email) && !StringExtensions.IsValidEmail(obj.email))
                {
                    return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Email không hợp lệ");
                }

                string hashedPassword = PasswordHelper.HashPassword(obj.password);

                var param = new DynamicParameters();
                param.Add("p_user_name", obj.user_name);
                param.Add("p_password", hashedPassword);
                param.Add("p_first_name", obj.first_name);
                param.Add("p_last_name", obj.last_name);
                param.Add("p_phone", obj.phone);
                param.Add("p_email", obj.email);
                param.Add("p_is_approved", obj.is_approved);
                param.Add("p_role_ids", obj.role_ids);

                var result = await Repository.ExecuteStoredProcPgSql("usr.user_create", param, "p_result");

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

        public async Task<CRUDResult<bool>> Update(UserUpdateReq obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj.email) && !StringExtensions.IsValidEmail(obj.email))
                {
                    return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Email không hợp lệ");
                }

                string hashedPassword = PasswordHelper.HashPassword(obj.password);

                var param = new DynamicParameters();
                param.Add("p_user_id", obj.user_id);
                param.Add("p_user_name", obj.user_name);
                param.Add("p_password", hashedPassword);
                param.Add("p_first_name", obj.first_name);
                param.Add("p_last_name", obj.last_name);
                param.Add("p_phone", obj.phone);
                param.Add("p_email", obj.email);
                param.Add("p_is_approved", obj.is_approved);
                param.Add("p_role_ids", obj.role_ids);

                var result = await Repository.ExecuteStoredProcPgSql("usr.user_update", param, "p_result");

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

        public async Task<CRUDResult<bool>> Delete(int userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_user_id", userId);

                var result = await Repository.ExecuteStoredProcPgSql("usr.user_delete", param, "p_result");

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
