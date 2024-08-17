using Dapper;
using EnvTracker.Application.Common;
using EnvTracker.Application.DTOs.Request.USR.Account;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.USR.Account;
using EnvTracker.Application.Services.Interfaces.USR;
using EnvTracker.Application.Utilities;
using EnvTracker.Domain.Entities;
using EnvTracker.Domain.Interfaces;
using Npgsql;

namespace EnvTracker.Application.Services.Implements.USR
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly IRoleService _roleService;

        public AccountService(IRepository repository, ITokenService jwtProvider, IRoleService roleService)
            : base(repository)
        {
            _tokenService = jwtProvider;
            _roleService = roleService;
        }

        public async Task<CRUDResult<bool>> SignUp(AccountSignUpReq obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj.email) && !StringExtensions.IsValidEmail(obj.email))
                {
                    return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Email không hợp lệ");
                }

                string hashedPassword = PasswordHelper.HashPassword(obj.password);

                var parameters = new DynamicParameters();
                parameters.Add("p_user_name", obj.user_name);
                parameters.Add("p_password", hashedPassword);
                parameters.Add("p_email", obj.email);
                parameters.Add("p_first_name", obj.first_name);
                parameters.Add("p_last_name", obj.last_name);
                parameters.Add("p_phone", obj.phone);

                var result = await Repository.ExecuteStoredProcPgSql("usr.user_sign_up", parameters, "p_result");

                if (result < 1)
                {
                    return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Dữ liệu chưa được cập nhật");
                }

                return Success(true);
            }
            catch (NpgsqlException ex)
            {
                return Error<bool>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<bool>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.GetExceptionMessage());
            }
        }

        public async Task<CRUDResult<string>> SignIn(AccountSignInReq obj)
        {
            try
            {
                string hashedPassword = PasswordHelper.HashPassword(obj.password);

                var parameters = new DynamicParameters();
                parameters.Add("p_user_name", obj.user_name);
                parameters.Add("p_password", hashedPassword);

                var result = await Repository.QueryFirstStoredProcPgSql<AccountSignInRes>("usr.user_sign_in", parameters, "p_result");

                if (result == null)
                {
                    return Error<string>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Email hoặc mật khẩu không đúng");
                }

                var roles = await _roleService.List();

                if (roles.StatusCode == CRUDStatusCodeRes.Success
                    && roles.Data != null
                    && roles.Data.Any())
                {
                    result.roles = roles.Data.Where(p => result.role_ids.Contains(p.role_id));
                }

                // Lọc ra các permission không trùng lặp
                var permissions = result.roles
                    .SelectMany(r => r.permissions) // Gộp tất cả danh sách permissions của các role lại thành một danh sách
                    .Distinct();

                var token = _tokenService.GenerateToken(new GenerateTokenReq
                {
                    user_name = result.user_name,
                    email = result.email,
                    full_name = result.first_name + " " + result.last_name,
                    phone = result.phone,
                    user_id = result.user_id,
                    permissions = permissions
                });

                return Success(token);
            }
            catch (NpgsqlException ex)
            {
                return Error<string>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<string>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.GetExceptionMessage());
            }
        }

        //public async Task<CRUDResult<AccountRes>> GetUserById(int user_id)
        //{
        //    try
        //    {
        //        var parameters = new DynamicParameters();
        //        parameters.Add("p_user_id", user_id);

        //        var result = await Repository.QueryFirstStoredProcPgSql<AccountRes>("usr.user_get_user_by_id", parameters, "p_result");

        //        if (result == null)
        //        {
        //            return Error<AccountRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
        //        }

        //        return Success(result);
        //    }
        //    catch (NpgsqlException ex)
        //    {
        //        return Error<AccountRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error<AccountRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.GetExceptionMessage());
        //    }
        //}

        //public async Task<CRUDResult<bool>> ValidateAccessToken(string token)
        //{
        //    var claims = _tokenService.Decode(token);
        //    var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);

        //    if (string.IsNullOrEmpty(userId?.Value))
        //        return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Invalid token");

        //    var user = await GetUserById(int.Parse(userId.Value));

        //    if (user == null)
        //        return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Invalid token");

        //    return Success(true);
        //}
    }
}
