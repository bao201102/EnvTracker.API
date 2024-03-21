using EnvTracker.Application.DTOs.Request.STA.SensorValue;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.STA.SensorValue;

namespace EnvTracker.Application.Services.Interfaces.STA
{
    public interface ISensorValueService : IDisposable
    {
        Task<PagingResponse<SensorValueSearchRes>> Search(SensorValueSearchReq obj);
        Task<CRUDResult<bool>> Create(SensorValueCreateReq obj);
    }
}
