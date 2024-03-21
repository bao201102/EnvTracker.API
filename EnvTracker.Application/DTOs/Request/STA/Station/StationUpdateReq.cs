using EnvTracker.Application.Common;

namespace EnvTracker.Application.DTOs.Request.STA.Station
{
    public class StationUpdateReq : BasePgDto
    {
        public int station_id { get; set; }
        public required string name { get; set; }
        public string? address { get; set; }
        public double latitude { get; set; }
        public double longtitude { get; set; }
        public bool is_active { get; set; } = true;
        public bool is_sms_alert_enable { get; set; } = false;
        public string? sms_alert_phone_number { get; set; }
        public string? param { get; set; }
        public int[]? sensor_ids { get; set; }
    }
}
