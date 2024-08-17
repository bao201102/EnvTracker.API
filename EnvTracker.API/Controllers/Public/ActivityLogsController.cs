using EnvTracker.API.Common;
using EnvTracker.Application.DTOs.Request.Public.ActivityLog;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.Public.ActivityLog;
using EnvTracker.Application.Services.Interfaces.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnvTracker.API.Controllers.Public
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityLogsController : BaseController
    {
        private readonly IActivityLogService _service;

        public ActivityLogsController(IActivityLogService service)
        {
            _service = service;
        }

        /// <summary>
        /// Search activity log
        /// </summary>
        /// 2024-08-17 - BaoNN
        /// <returns></returns>
        [HttpPost("search")]
        [ProducesResponseType(200, Type = typeof(PagingResponse<ActivityLogSearchRes>))]
        [Authorize]
        public async Task<IActionResult> Search(ActivityLogSearchReq obj)
        {
            var result = await _service.Search(obj);
            return ApiOK(result);
        }
    }
}
