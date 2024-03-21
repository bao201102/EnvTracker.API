namespace EnvTracker.Application.DTOs.Response.STA.Station
{
    public class StationChartRes
    {
        public int value_time_id { get; set; }
        public long time_epoch { get; set; }
        public DateTime? time { get; set; }
        public IEnumerable<StationChartSensorValues> sensor_values { get; set; }
    }

    public class StationChartSensorValues
    {
        public int sensor_value_id { get; set; }
        public int sensor_id { get; set; }
        public string sensor_name { get; set; }
        public double value { get; set; }
        public int value_time_id { get; set; }
    }
}
