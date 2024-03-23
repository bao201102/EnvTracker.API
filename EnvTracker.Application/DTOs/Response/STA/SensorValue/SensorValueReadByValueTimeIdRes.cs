namespace EnvTracker.Application.DTOs.Response.STA.SensorValue
{
    public class SensorValueReadByValueTimeIdRes
    {
        public int value_time_id { get; set; }
        public int station_id { get; set; }
        public string station_name { get; set; }
        public long time_epoch { get; set; }
        public DateTime time { get; set; }
        public IEnumerable<SensorValuesReadByValueTimeId> sensor_values { get; set; }
    }

    public class SensorValuesReadByValueTimeId
    {
        public int sensor_value_id { get; set; }
        public int sensor_id { get; set; }
        public string sensor_name { get; set; }
        public double value { get; set; }
    }
}
