using EnvTracker.Application.Utilities;
using Newtonsoft.Json;

namespace EnvTracker.Application.DTOs.Response.STA.SensorValue
{
    public class SensorValueSearchRes
    {
        public int total_record { get; set; }
        public int value_time_id { get; set; }
        public long time_epoch { get; set; }
        public DateTime? time { get; set; }
        public int station_id { get; set; }
        public IEnumerable<SensorValues> sensor_values { get; set; }
    }

    public class SensorValues
    {
        public int sensor_value_id { get; set; }
        public int sensor_id { get; set; }
        public string sensor_name { get; set; }
        public double value { get; set; }
        public int value_time_id { get; set; }
    }
}
