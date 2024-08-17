using EnvTracker.API.Common;
using EnvTracker.Application.DTOs.Request.USR.Role;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.USR.Role;
using EnvTracker.Application.Services.Interfaces.USR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnvTracker.API.Controllers.USR
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : BaseController
    {
        private readonly IRoleService _service;

        public RolesController(IRoleService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all role
        /// </summary>
        /// 2024-08-17 - BaoNN
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoleRes>))]
        [Authorize]
        public async Task<IActionResult> List()
        {
            var result = await _service.List();
            return ApiOK(result);
        }

        /// <summary>
        /// Get Role by id
        /// </summary>
        /// 2024-03-23 - BaoNN
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        [ProducesResponseType(200, Type = typeof(RoleRes))]
        [Authorize]
        public async Task<IActionResult> ReadById(int roleId)
        {
            var result = await _service.ReadById(roleId);
            return ApiOK(result);
        }

        /// <summary>
        /// Search Role
        /// </summary>
        /// 2024-03-23 - BaoNN
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("search")]
        [ProducesResponseType(200, Type = typeof(PagingResponse<RoleSearchRes>))]
        [Authorize]
        public async Task<IActionResult> Search(RoleSearchReq obj)
        {
            var result = await _service.Search(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Create Role
        /// </summary>
        /// 2024-03-23 - BaoNN
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Create(RoleCreateReq obj)
        {
            var result = await _service.Create(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Update Role
        /// </summary>
        /// 2024-03-23 - BaoNN
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Update(RoleUpdateReq obj)
        {
            var result = await _service.Update(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Delete Role
        /// </summary>
        /// 2024-03-23 - BaoNN
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("{roleId}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Delete(int roleId)
        {
            var result = await _service.Delete(roleId);
            return ApiOK(result);
        }
    }
}
