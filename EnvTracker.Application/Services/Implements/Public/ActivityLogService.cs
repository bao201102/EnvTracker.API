using EnvTracker.Application.Common;
using EnvTracker.Application.DTOs.Request.Public.ActivityLog;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.Public.ActivityLog;
using EnvTracker.Application.DTOs.Response.STA.Sensor;
using EnvTracker.Application.Services.Interfaces.Public;
using EnvTracker.Application.Utilities;
using EnvTracker.Domain.Interfaces;
using Npgsql;

namespace EnvTracker.Application.Services.Implements
{
    public class ActivityLogService : BaseService, IActivityLogService
    {
        public ActivityLogService(IRepository repository)
            : base(repository)
        {
        }

        public async Task<PagingResponse<ActivityLogSearchRes>> Search(ActivityLogSearchReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.QueryStoredProcPgSql<ActivityLogSearchRes>("public.activity_log_search", param, "p_result");
                if (result == null || !result.Any())
                {
                    return new PagingResponse<ActivityLogSearchRes>
                    {
                        StatusCode = CRUDStatusCodeRes.ResourceNotFound
                    };
                }

                return PagingSuccess(result, obj.page_index, obj.page_size, result.FirstOrDefault().total_record);
            }
            catch (NpgsqlException ex)
            {
                return PagingError<ActivityLogSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return PagingError<ActivityLogSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.GetExceptionMessage());
            }
        }

        public async Task<CRUDResult<bool>> Create(ActivityLogCreateReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.ExecuteStoredProcPgSql("public.activity_log_create", param, "p_result");

                if (result < 1)
                {
                    return Error(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Lỗi cập nhật dữ liệu", data: false);
                }

                return Success(true);
            }
            catch (NpgsqlException ex)
            {
                return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage(), data: false);
            }
            catch (Exception ex)
            {
                return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message, data: false);
            }
        }

    }
}
