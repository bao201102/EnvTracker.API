using EnvTracker.API.Common;
using EnvTracker.Application.DTOs.Request.STA.Sensor;
using EnvTracker.Application.DTOs.Request.STA.Station;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.STA.Sensor;
using EnvTracker.Application.DTOs.Response.STA.Station;
using EnvTracker.Application.Services.Interfaces.STA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnvTracker.API.Controllers.STA
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : BaseController
    {
        private readonly ISensorService _service;

        public SensorsController(ISensorService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all Sensors
        /// </summary>
        /// 2024-03-09 - BaoNN
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SensorRes>))]
        [Authorize]
        public async Task<IActionResult> List()
        {
            var result = await _service.List();
            return ApiOK(result);
        }

        /// <summary>
        /// Get sensor
        /// </summary>
        /// <param name="sensorId"></param>
        /// <returns></returns>
        [HttpGet("{sensorId}")]
        [ProducesResponseType(200, Type = typeof(SensorRes))]
        [Authorize]
        public async Task<IActionResult> ReadById(int sensorId)
        {
            var result = await _service.ReadById(sensorId);
            return ApiOK(result);
        }

        /// <summary>
        /// Search Sensors
        /// </summary>
        /// 2024-03-17 - BaoNN
        /// <returns></returns>
        [HttpPost("search")]
        [ProducesResponseType(200, Type = typeof(PagingResponse<SensorSearchRes>))]
        [Authorize]
        public async Task<IActionResult> Search(SensorSearchReq obj)
        {
            var result = await _service.Search(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Create Sensor
        /// </summary>
        /// 2024-03-10 - BaoNN
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Create(SensorCreateReq obj)
        {
            var result = await _service.Create(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Update sensor
        /// </summary>
        /// 2024-03-15 - BaoNN
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Update(SensorUpdateReq obj)
        {
            var result = await _service.Update(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Delete Sensor
        /// </summary>
        /// 2024-03-15 - BaoNN
        /// <param name="sensorId"></param>
        /// <returns></returns>
        [HttpDelete("{sensorId}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Delete(int sensorId)
        {
            var result = await _service.Delete(sensorId);
            return ApiOK(result);
        }
    }
}
