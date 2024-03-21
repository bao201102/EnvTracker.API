using EnvTracker.Application.DTOs.Request.USR.Account;
using EnvTracker.Application.DTOs.Response.Common;

namespace EnvTracker.Application.Services.Interfaces.USR
{
    public interface IAccountService : IDisposable
    {
        Task<CRUDResult<bool>> SignUp(AccountSignUpReq obj);
        Task<CRUDResult<string>> SignIn(AccountSignInReq obj);
    }
}
