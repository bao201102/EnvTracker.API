using EnvTracker.Application.DTOs.Request.USR.User;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.USR.User;

namespace EnvTracker.Application.Services.Interfaces.USR
{
    public interface IUserService : IDisposable
    {
        Task<CRUDResult<UserRes>> ReadById(int userId);
        Task<PagingResponse<UserSearchRes>> Search(UserSearchReq obj);
        Task<CRUDResult<bool>> Create(UserCreateReq obj);
        Task<CRUDResult<bool>> Update(UserUpdateReq obj);
        Task<CRUDResult<bool>> Delete(int userId);
    }
}
