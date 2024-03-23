using EnvTracker.API.Common;
using EnvTracker.Application.DTOs.Request.USR.User;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.USR.User;
using EnvTracker.Application.Services.Interfaces.USR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnvTracker.API.Controllers.USR
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get User by User Id
        /// </summary>
        /// 2024-03-23 - BaoNN
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(UserRes))]
        [Authorize]
        public async Task<IActionResult> ReadById(int userId)
        {
            var result = await _service.ReadById(userId);
            return ApiOK(result);
        }

        /// <summary>
        /// Search User
        /// </summary>
        /// 2024-03-23 - BaoNN
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("search")]
        [ProducesResponseType(200, Type = typeof(PagingResponse<UserSearchRes>))]
        [Authorize]
        public async Task<IActionResult> Search(UserSearchReq obj)
        {
            var result = await _service.Search(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// 2024-03-23 - BaoNN
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Create(UserCreateReq obj)
        {
            var result = await _service.Create(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// 2024-03-23 - BaoNN
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Update(UserUpdateReq obj)
        {
            var result = await _service.Update(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// Delete Sensor Value
        /// </summary>
        /// 2024-03-23 - BaoNN
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Authorize]
        public async Task<IActionResult> Delete(int userId)
        {
            var result = await _service.Delete(userId);
            return ApiOK(result);
        }
    }
}
