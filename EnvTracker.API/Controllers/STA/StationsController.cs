using EnvTracker.API.Common;
using EnvTracker.Application.DTOs.Request.STA.Station;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.STA.Station;
using EnvTracker.Application.Services.Interfaces.STA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnvTracker.API.Controllers.STA
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationsController : BaseController
    {
        private readonly IStationService _service;

        public StationsController(IStationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all Stations
        /// </summary>
        /// 2024-03-09 - BaoNN
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StationRes>))]
        [Authorize]
        public async Task<IActionResult> List()
        {
            var result = await _service.List();
            return ApiOK(result);
        }

        /// <summary>
        /// Get Station by station id
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [HttpGet("{stationId}")]
        [ProducesResponseType(200, Type = typeof(StationRes))]
        [Authorize]
        public async Task<IActionResult> ReadById(int stationId)
        {
            var result = await _service.ReadById(stationId);
            return ApiOK(result);
        }

        /// <summary>
        /// Search Stations
        /// </summary>
        /// 2024-03-17 - BaoNN
        /// <returns></returns>
        [HttpPost("search")]
        [ProducesResponseType(200, Type = typeof(PagingResponse<StationSearchRes>))]
        [Authorize]
        public async Task<IActionResult> Search(StationSearchReq obj)
        {
            var result = await _service.Search(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Chart Sensor value by Station id
        /// </summary>
        /// 2024-03-19 - BaoNN
        /// <returns></returns>
        [HttpPost("chart")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StationChartRes>))]
        [Authorize]
        public async Task<IActionResult> Chart(StationChartReq obj)
        {
            var result = await _service.Chart(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Create Station
        /// </summary>
        /// 2024-03-10 - BaoNN
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Create(StationCreateReq obj)
        {
            var result = await _service.Create(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Update Station by station id
        /// </summary>
        /// 2024-03-15 - BaoNN
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Update(StationUpdateReq obj)
        {
            var result = await _service.Update(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Delete Station
        /// </summary>
        /// 2024-03-15 - BaoNN
        /// <param name="stationId"></param>
        /// <returns></returns>
        [HttpDelete("{stationId}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Delete(int stationId)
        {
            var result = await _service.Delete(stationId);
            return ApiOK(result);
        }
    }
}
