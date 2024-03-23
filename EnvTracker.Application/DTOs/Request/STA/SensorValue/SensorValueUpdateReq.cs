namespace EnvTracker.Application.DTOs.Request.STA.SensorValue
{
    public class SensorValueUpdateReq
    {
        public int value_time_id { get; set; }
        public IEnumerable<SensorValuesUpdate> sensor_values { get; set; }
    }

    public class SensorValuesUpdate
    {
        public int sensor_value_id { get; set; }
        public double value { get; set; }
    }
}
