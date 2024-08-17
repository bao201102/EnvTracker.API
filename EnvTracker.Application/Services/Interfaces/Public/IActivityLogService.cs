using EnvTracker.Application.DTOs.Request.Public.ActivityLog;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.Public.ActivityLog;

namespace EnvTracker.Application.Services.Interfaces.Public
{
    public interface IActivityLogService : IDisposable
    {
        Task<PagingResponse<ActivityLogSearchRes>> Search(ActivityLogSearchReq obj);
        Task<CRUDResult<bool>> Create(ActivityLogCreateReq obj);
    }
}
