using EnvTracker.API.Common;
using EnvTracker.Application.DTOs.Request.STA.SensorValue;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.STA.SensorValue;
using EnvTracker.Application.Services.Interfaces.STA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnvTracker.API.Controllers.STA
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorValuesController : BaseController
    {
        private readonly ISensorValueService _service;

        public SensorValuesController(ISensorValueService service)
        {
            _service = service;
        }

        /// <summary>
        /// Search Sensor Values
        /// </summary>
        /// 2024-03-14 - BaoNN
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("search")]
        [ProducesResponseType(200, Type = typeof(PagingResponse<SensorValueSearchRes>))]
        [Authorize]
        public async Task<IActionResult> Search(SensorValueSearchReq obj)
        {
            var result = await _service.Search(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Create Sensor Value
        /// </summary>
        /// 2024-03-14 - BaoNN
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Create(SensorValueCreateReq obj)
        {
            var result = await _service.Create(obj);
            return ApiOK(result);
        }
    }
}
