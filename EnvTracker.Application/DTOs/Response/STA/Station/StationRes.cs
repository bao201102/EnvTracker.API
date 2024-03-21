namespace EnvTracker.Application.DTOs.Response.STA.Station
{
    public class StationRes
    {
        public int station_id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public double latitude { get; set; }
        public double longtitude { get; set; }
        public bool is_active { get; set; }
        public bool is_sms_alert_enable { get; set; }
        public string sms_alert_phone_number { get; set; }
        public string param { get; set; }
        public int[] sensor_ids { get; set; }

        public IEnumerable<StationSensorRes> sensors { get; set; }
    }

    public class StationSensorRes
    {
        public int sensor_id { get; set; }
        public string sensor_name { get; set; }
    }
}
