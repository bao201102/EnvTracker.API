namespace EnvTracker.Application.DTOs.Request.STA.SensorValue
{
    public class SensorValueCreateReq
    {
        public int station_id { get; set; }
        public DateTime time { get; set; } // Value time
        public IEnumerable<SensorValuesCreate> sensor_values { get; set; }
    }

    public class SensorValuesCreate
    {
        public int sensor_id { get; set; }
        public double value { get; set; }
    }
}
