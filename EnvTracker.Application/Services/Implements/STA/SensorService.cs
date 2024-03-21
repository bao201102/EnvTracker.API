using Dapper;
using EnvTracker.Application.Common;
using EnvTracker.Application.DTOs.Request.STA.Sensor;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.STA.Sensor;
using EnvTracker.Application.Services.Interfaces.STA;
using EnvTracker.Application.Utilities;
using EnvTracker.Domain.Interfaces;
using Npgsql;

namespace EnvTracker.Application.Services.Implements.STA
{
    public class SensorService : BaseService, ISensorService
    {
        public SensorService(IRepository repository)
            : base(repository)
        {
        }

        public async Task<CRUDResult<IEnumerable<SensorRes>>> List()
        {
            try
            {
                var result = await Repository.QueryStoredProcPgSql<SensorRes>("sta.sensor_read_all", null, "p_result");

                if (result == null || !result.Any())
                {
                    return Error<IEnumerable<SensorRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                return Success(result);
            }
            catch (NpgsqlException ex)
            {
                return Error<IEnumerable<SensorRes>>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<IEnumerable<SensorRes>>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message);
            }
        }

        public async Task<CRUDResult<SensorRes>> ReadById(int sensorId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_sensor_id", sensorId);

                var result = await Repository.QueryFirstStoredProcPgSql<SensorRes>("sta.sensor_read_by_id", param, "p_result");

                if (result == null)
                {
                    return Error<SensorRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                return Success(result);
            }
            catch (NpgsqlException ex)
            {
                return Error<SensorRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<SensorRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message);
            }
        }

        public async Task<PagingResponse<SensorSearchRes>> Search(SensorSearchReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.QueryStoredProcPgSql<SensorSearchRes>("sta.sensor_search", param, "p_result");
                if (result == null || !result.Any())
                {
                    return new PagingResponse<SensorSearchRes>
                    {
                        StatusCode = CRUDStatusCodeRes.ResourceNotFound
                    };
                }

                return PagingSuccess(result, obj.page_index, obj.page_size, result.FirstOrDefault().total_record);
            }
            catch (NpgsqlException ex)
            {
                return PagingError<SensorSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return PagingError<SensorSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.GetExceptionMessage());
            }
        }

        public async Task<CRUDResult<bool>> Create(SensorCreateReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.ExecuteStoredProcPgSql("sta.sensor_create", param, "p_result");

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

        public async Task<CRUDResult<bool>> Update(SensorUpdateReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.ExecuteStoredProcPgSql("sta.sensor_update", param, "p_result");

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

        public async Task<CRUDResult<bool>> Delete(int sensorId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_sensor_id", sensorId);

                var result = await Repository.ExecuteStoredProcPgSql("sta.sensor_delete", param, "p_result");

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
