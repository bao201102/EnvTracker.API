using Dapper;
using EnvTracker.Application.Common;
using EnvTracker.Application.DTOs.Request.STA.Station;
using EnvTracker.Application.DTOs.Response.Common;
using EnvTracker.Application.DTOs.Response.STA.Station;
using EnvTracker.Application.Services.Interfaces.STA;
using EnvTracker.Application.Utilities;
using EnvTracker.Domain.Interfaces;
using Npgsql;

namespace EnvTracker.Application.Services.Implements.STA
{
    public class StationService : BaseService, IStationService
    {
        private readonly ISensorService _sensorService;

        public StationService(IRepository repository, ISensorService sensorService)
            : base(repository)
        {
            _sensorService = sensorService;
        }

        public async Task<CRUDResult<IEnumerable<StationRes>>> List()
        {
            try
            {
                var result = await Repository.QueryStoredProcPgSql<StationRes>("sta.station_read_all", null, "p_result");

                if (result == null || !result.Any())
                {
                    return Error<IEnumerable<StationRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                var sensorList = await _sensorService.List();
                if (sensorList.StatusCode != CRUDStatusCodeRes.Success)
                {
                    return Error<IEnumerable<StationRes>>(statusCode: sensorList.StatusCode, errorMessage: sensorList.ErrorMessage);
                }

                result = result.Select(st =>
                {
                    st.sensors = sensorList.Data
                        .Where(s => st.sensor_ids != null && st.sensor_ids.Any(id => id == s.sensor_id))
                        .Select(s => new StationSensorRes
                        {
                            sensor_id = s.sensor_id,
                            sensor_name = s.name
                        });

                    return st;
                });

                return Success(result);
            }
            catch (NpgsqlException ex)
            {
                return Error<IEnumerable<StationRes>>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<IEnumerable<StationRes>>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message);
            }
        }

        public async Task<CRUDResult<StationRes>> ReadById(int stationId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_station_id", stationId);

                var result = await Repository.QueryFirstStoredProcPgSql<StationRes>("sta.station_read_by_id", param, "p_result");

                if (result == null)
                {
                    return Error<StationRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                var sensorList = await _sensorService.List();
                if (sensorList.StatusCode != CRUDStatusCodeRes.Success)
                {
                    return Error<StationRes>(statusCode: sensorList.StatusCode, errorMessage: sensorList.ErrorMessage);
                }

                result.sensors = sensorList.Data.Select(s => new StationSensorRes
                {
                    sensor_id = s.sensor_id,
                    sensor_name = s.name
                }).Where(s => result.sensor_ids != null && result.sensor_ids.Any(id => id == s.sensor_id));

                return Success(result);
            }
            catch (NpgsqlException ex)
            {
                return Error<StationRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<StationRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message);
            }
        }

        public async Task<PagingResponse<StationSearchRes>> Search(StationSearchReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.QueryStoredProcPgSql<StationSearchRes>("sta.station_search", param, "p_result");
                if (result == null || !result.Any())
                {
                    return new PagingResponse<StationSearchRes>
                    {
                        StatusCode = CRUDStatusCodeRes.ResourceNotFound
                    };
                }

                var sensorList = await _sensorService.List();
                if (sensorList.StatusCode != CRUDStatusCodeRes.Success)
                {
                    return new PagingResponse<StationSearchRes>
                    {
                        StatusCode = sensorList.StatusCode,
                        ErrorMessage = sensorList.ErrorMessage
                    };
                }

                result = result.Select(st =>
                {
                    st.sensors = sensorList.Data
                        .Where(s => st.sensor_ids != null && st.sensor_ids.Any(id => id == s.sensor_id))
                        .Select(s => new StationSearchSensors
                        {
                            sensor_id = s.sensor_id,
                            sensor_name = s.name
                        });

                    return st;
                });

                return PagingSuccess(result, obj.page_index, obj.page_size, result.FirstOrDefault().total_record);
            }
            catch (NpgsqlException ex)
            {
                return PagingError<StationSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return PagingError<StationSearchRes>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.GetExceptionMessage());
            }
        }

        public async Task<CRUDResult<IEnumerable<StationChartRes>>> Chart(StationChartReq obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_station_id", obj.station_id);
                param.Add("p_date_from", obj.date_from.DateTimeToUnixTimestamp());
                param.Add("p_date_to", obj.date_to.DateTimeToUnixTimestamp());

                var result = await Repository.QueryMultiStoredProcPgSql("sta.station_chart", param, "p_result", "p_result1");

                var lstValueTime = await result.ReadAsync<StationChartRes>();
                var lstSensorValue = await result.ReadAsync<StationChartSensorValues>();

                if (lstValueTime == null || !lstValueTime.Any() || lstSensorValue == null || !lstSensorValue.Any())
                {
                    return Error<IEnumerable<StationChartRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                var chartResult = lstValueTime.Select(vt =>
                {
                    vt.time = vt.time_epoch.UnixTimestampToDateTime();
                    vt.sensor_values = lstSensorValue
                        .Where(sv => sv.value_time_id == vt.value_time_id);

                    return vt;
                });


                return Success(chartResult);
            }
            catch (NpgsqlException ex)
            {
                return Error<IEnumerable<StationChartRes>>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message.ConvertNpgsqlExceptionMessage());
            }
            catch (Exception ex)
            {
                return Error<IEnumerable<StationChartRes>>(statusCode: CRUDStatusCodeRes.ResetContent, errorMessage: ex.Message);
            }
        }

        public async Task<CRUDResult<bool>> Create(StationCreateReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.ExecuteStoredProcPgSql("sta.station_create", param, "p_result");

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

        public async Task<CRUDResult<bool>> Update(StationUpdateReq obj)
        {
            try
            {
                var param = obj.ToDynamicParameters();

                var result = await Repository.ExecuteStoredProcPgSql("sta.station_update", param, "p_result");

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

        public async Task<CRUDResult<bool>> Delete(int stationId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_station_id", stationId);

                var result = await Repository.ExecuteStoredProcPgSql("sta.station_delete", param, "p_result");

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
