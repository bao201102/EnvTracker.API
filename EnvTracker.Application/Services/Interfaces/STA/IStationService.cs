using EnvTracker.Application.DTOs.Request.STA.Station;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.STA.Station;

namespace EnvTracker.Application.Services.Interfaces.STA
{
    public interface IStationService : IDisposable
    {
        Task<CRUDResult<IEnumerable<StationRes>>> List();
        Task<CRUDResult<StationRes>> ReadById(int stationId);
        Task<PagingResponse<StationSearchRes>> Search(StationSearchReq obj);
        Task<CRUDResult<IEnumerable<StationChartRes>>> Chart(StationChartReq obj);
        Task<CRUDResult<bool>> Create(StationCreateReq obj);
        Task<CRUDResult<bool>> Update(StationUpdateReq obj);
        Task<CRUDResult<bool>> Delete(int stationId);
    }
}
