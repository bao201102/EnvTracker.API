using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EnvTracker.API.Common
{
    public abstract class BaseController : ControllerBase
    {
        private UserPrincipal _currentUser;

        protected UserPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = new UserPrincipal(HttpContext.User.Claims.ToList());
                }

                return _currentUser;
            }
        }

        protected IActionResult ApiOK<T>(CRUDResult<T> obj)
        {
            if (obj.StatusCode == CRUDStatusCodeRes.Success)
            {
                return Ok(obj.Data);
            }

            if (obj.StatusCode == CRUDStatusCodeRes.ReturnWithData)
            {
                return Created(string.Empty, obj.Data);
            }

            if (obj.StatusCode == CRUDStatusCodeRes.ResourceNotFound || obj.StatusCode == CRUDStatusCodeRes.Deny)
            {
                return NoContent();
            }

            if (obj.StatusCode == CRUDStatusCodeRes.InvalidData || obj.StatusCode == CRUDStatusCodeRes.ResetContent)
            {
                return StatusCode(406, obj.ErrorMessage);
            }

            return StatusCode(500, obj.ErrorMessage);
        }

        protected IActionResult ApiOK<T>(PagingResponse<T> obj)
        {
            if (obj.StatusCode == CRUDStatusCodeRes.Success)
            {
                return Ok(obj);
            }

            if (obj.StatusCode == CRUDStatusCodeRes.ReturnWithData)
            {
                return Created(string.Empty, obj);
            }

            if (obj.StatusCode == CRUDStatusCodeRes.ResourceNotFound || obj.StatusCode == CRUDStatusCodeRes.Deny)
            {
                return NoContent();
            }

            if (obj.StatusCode == CRUDStatusCodeRes.InvalidData || obj.StatusCode == CRUDStatusCodeRes.ResetContent)
            {
                return StatusCode(406, obj.ErrorMessage);
            }

            return StatusCode(500, obj.ErrorMessage);
        }

        //protected IActionResult CreateResponse<T>(CRUDResult<T> obj)
        //{
        //    if (obj.StatusCode == CRUDStatusCodeRes.Success)
        //    {
        //        return Ok(obj.Data);
        //    }

        //    if (obj.StatusCode == CRUDStatusCodeRes.ReturnWithData)
        //    {
        //        return Created(string.Empty, obj.Data);
        //    }

        //    if (obj.StatusCode == CRUDStatusCodeRes.ResourceNotFound || obj.StatusCode == CRUDStatusCodeRes.Deny)
        //    {
        //        return NoContent();
        //    }

        //    if (obj.StatusCode == CRUDStatusCodeRes.InvalidData || obj.StatusCode == CRUDStatusCodeRes.ResetContent)
        //    {
        //        return StatusCode(406, obj.ErrorMessage);
        //    }

        //    return StatusCode(500, obj.ErrorMessage);
        //}

        //[NonAction]
        //protected IActionResult CreateResponse<T>(PagingResponse<T> obj)
        //{
        //    if (obj.StatusCode == CRUDStatusCodeRes.Success)
        //    {
        //        return Ok(obj);
        //    }

        //    if (obj.StatusCode == CRUDStatusCodeRes.ResourceNotFound || obj.StatusCode == CRUDStatusCodeRes.Deny)
        //    {
        //        return NoContent();
        //    }

        //    if (obj.StatusCode == CRUDStatusCodeRes.InvalidData || obj.StatusCode == CRUDStatusCodeRes.ResetContent)
        //    {
        //        return StatusCode(406, obj.ErrorMessage);
        //    }

        //    return StatusCode(500, obj.ErrorMessage);
        //}
    }
}
