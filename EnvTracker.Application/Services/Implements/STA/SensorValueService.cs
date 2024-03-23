using Dapper;
using EnvTracker.Application.Common;
using EnvTracker.Application.DTOs.Request.STA.SensorValue;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.STA.Sensor;
using EnvTracker.Application.DTOs.Response.STA.SensorValue;
using EnvTracker.Application.Services.Interfaces.STA;
using EnvTracker.Application.Utilities;
using EnvTracker.Domain.Interfaces;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace EnvTracker.Application.Services.Implements.STA
{
    public class SensorValueService : BaseService, ISensorValueService
    {
        public SensorValueService(IRepository repository)
            : base(repository)
        {
        }

        public async Task<CRUDResult<SensorValueReadByValueTimeIdRes>> ReadByValueTimeId(int valueTimeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_value_time_id", valueTimeId);

                var result = await Repository.QueryMultiStoredProcPgSql("sta.sensor_value_read_by_value_time_id", param, "p_result", "p_result1");

                var lstValueTime = await result.ReadFirstOrDefaultAsync<SensorValueReadByValueTimeIdRes>();
                var lstSensorValue = await result.ReadAsync<SensorValuesReadByValueTimeId>();

                if (lstValueTime == null)
                {
                    return Error<SensorValueReadByValueTimeIdRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                lstValueTime.time = lstValueTime.time_epoch.UnixTimestampToDateTime();
                lstValueTime.sensor_values = lstSensorValue;

                return Success(lstValueTime);
            }
            catch (NpgsqlException ex)
            {
                return Error<SensorValueReadByValueTimeIdRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<SensorValueReadByValueTimeIdRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message);
            }
        }

        public async Task<PagingResponse<SensorValueSearchRes>> Search(SensorValueSearchReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.QueryMultiStoredProcPgSql("sta.sensor_value_search", param, "p_result", "p_result1");

                var lstValueTime = await result.ReadAsync<SensorValueSearchRes>();
                var lstSensorValue = await result.ReadAsync<SensorValues>();

                if (lstValueTime == null || !lstValueTime.Any())
                {
                    return new PagingResponse<SensorValueSearchRes>
                    {
                        StatusCode = CRUDStatusCodeRes.ResourceNotFound
                    };
                }

                var searchResult = lstValueTime.Select(vt =>
                {
                    vt.time = vt.time_epoch.UnixTimestampToDateTime();
                    vt.sensor_values = lstSensorValue
                        .Where(sv => sv.value_time_id == vt.value_time_id);

                    return vt;
                });

                return PagingSuccess(searchResult, obj.page_index, obj.page_size, searchResult.FirstOrDefault().total_record);
            }
            catch (NpgsqlException ex)
            {
                return PagingError<SensorValueSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return PagingError<SensorValueSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.GetExceptionMessage());
            }
        }

        public async Task<CRUDResult<bool>> Create(SensorValueCreateReq obj)
        {
            var param = new DynamicParameters();
            param.Add("p_station_id", obj.station_id);
            param.Add("p_time", obj.time.DateTimeToUnixTimestamp());
            param.Add("p_sensor_values", JsonConvert.SerializeObject(obj.sensor_values));

            await Repository.Reconnect();
            using (var tran = Repository.Connection.BeginTransaction())
            {
                try
                {
                    var result = await Repository.ExecuteStoredProcPgSql("sta.sensor_value_create", param, "p_result", tran);

                    if (result < 1)
                    {
                        tran.Rollback();
                        return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: "Lỗi cập nhật dữ liệu", data: false);
                    }

                    tran.Commit();
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

        public async Task<CRUDResult<bool>> Update(SensorValueUpdateReq obj)
        {
            var param = new DynamicParameters();
            param.Add("p_value_time_id", obj.value_time_id);
            param.Add("p_sensor_values", JsonConvert.SerializeObject(obj.sensor_values));

            await Repository.Reconnect();
            using (var tran = Repository.Connection.BeginTransaction())
            {
                try
                {
                    var result = await Repository.ExecuteStoredProcPgSql("sta.sensor_value_update", param, "p_result", tran);

                    if (result < 1)
                    {
                        tran.Rollback();
                        return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: "Lỗi cập nhật dữ liệu", data: false);
                    }

                    tran.Commit();
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

        public async Task<CRUDResult<bool>> Delete(int valueTimeId)
        {
            var param = new DynamicParameters();
            param.Add("p_value_time_id", valueTimeId);

            await Repository.Reconnect();
            using (var tran = Repository.Connection.BeginTransaction())
            {
                try
                {
                    var result = await Repository.ExecuteStoredProcPgSql("sta.sensor_value_delete", param, "p_result", tran);

                    if (result < 1)
                    {
                        tran.Rollback();
                        return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: "Lỗi cập nhật dữ liệu", data: false);
                    }

                    tran.Commit();
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
}
