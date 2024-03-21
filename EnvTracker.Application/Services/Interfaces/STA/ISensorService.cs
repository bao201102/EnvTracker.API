using EnvTracker.Application.DTOs.Request.STA.Sensor;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.STA.Sensor;

namespace EnvTracker.Application.Services.Interfaces.STA
{
    public interface ISensorService : IDisposable
    {
        Task<CRUDResult<IEnumerable<SensorRes>>> List();
        Task<CRUDResult<SensorRes>> ReadById(int sensorId);
        Task<PagingResponse<SensorSearchRes>> Search(SensorSearchReq obj);
        Task<CRUDResult<bool>> Create(SensorCreateReq obj);
        Task<CRUDResult<bool>> Update(SensorUpdateReq obj);
        Task<CRUDResult<bool>> Delete(int sensorId);
    }
}
