using EnvTracker.API.Common;
using EnvTracker.Application.DTOs.Response.USR.Permission;
using EnvTracker.Application.Services.Interfaces.USR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnvTracker.API.Controllers.USR
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : BaseController
    {
        private readonly IPermissionService _service;

        public PermissionsController(IPermissionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all permission
        /// </summary>
        /// 2024-08-17 - BaoNN
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PermissionRes>))]
        [Authorize]
        public async Task<IActionResult> List()
        {
            var result = await _service.List();
            return ApiOK(result);
        }
    }
}
