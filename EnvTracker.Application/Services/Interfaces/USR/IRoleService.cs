using EnvTracker.Application.DTOs.Request.USR.Role;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.USR.Role;

namespace EnvTracker.Application.Services.Interfaces.USR
{
    public interface IRoleService : IDisposable
    {
        Task<CRUDResult<RoleRes>> ReadById(int roleId);
        Task<PagingResponse<RoleSearchRes>> Search(RoleSearchReq obj);
        Task<CRUDResult<bool>> Create(RoleCreateReq obj);
        Task<CRUDResult<bool>> Update(RoleUpdateReq obj);
        Task<CRUDResult<bool>> Delete(int roleId);
    }
}
