using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.USR.Permission;

namespace EnvTracker.Application.Services.Interfaces.USR
{
    public interface IPermissionService : IDisposable
    {
        Task<CRUDResult<IEnumerable<PermissionRes>>> List();
    }
}
