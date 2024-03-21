using Dapper;
using EnvTracker.Application.Common;
using EnvTracker.Application.DTOs.Request.STA.SensorValue;
using EnvTracker.Application.DTOs.Response.Common;
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

        public async Task<PagingResponse<SensorValueSearchRes>> Search(SensorValueSearchReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.QueryMultiStoredProcPgSql("sta.sensor_value_search", param, "p_result", "p_result1");

                var lstValueTime = await result.ReadAsync<SensorValueSearchRes>();
                var lstSensorValue = await result.ReadAsync<SensorValues>();

                if (lstValueTime == null || !lstValueTime.Any() || lstSensorValue == null || !lstSensorValue.Any())
                {
                    return new PagingResponse<SensorValueSearchRes>
                    {
                        StatusCode = CRUDStatusCodeRes.ResourceNotFound
                    };
                }

                var searchResult = lstValueTime.Select(vt => new SensorValueSearchRes
                {
                    total_record = vt.total_record,
                    value_time_id = vt.value_time_id,
                    time = vt.time_epoch.UnixTimestampToDateTime(),
                    time_epoch = vt.time_epoch,
                    station_id = vt.station_id,
                    sensor_values = lstSensorValue
                        .Where(sv => sv.value_time_id == vt.value_time_id)
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
                        return Error(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: "Lỗi thêm dữ liệu", data: false);
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
