using EnvTracker.Application.Common;

namespace EnvTracker.Application.DTOs.Request.STA.Sensor
{
    public class SensorCreateReq : BasePgDto
    {
        public required string name { get; set; }
        public double? upper_bound { get; set; }
        public double? lower_bound { get; set; }
        public double? differ_bound { get; set; }
        public string? unit_symbol { get; set; }
        public bool is_active { get; set; } = true;
        public string? param { get; set; }
    }
}
