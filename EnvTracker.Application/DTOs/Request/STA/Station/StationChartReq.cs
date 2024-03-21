using EnvTracker.Application.Common;
using FluentValidation;
using System.Data.SqlTypes;

namespace EnvTracker.Application.DTOs.Request.STA.Station
{
    public class StationChartReq : BasePgDto
    {
        public int station_id { get; set; }
        public DateTime date_from { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime date_to { get; set; } = DateTime.Now;
    }

    public class StationChartReqValidator : AbstractValidator<StationChartReq>
    {
        public StationChartReqValidator()
        {
            RuleFor(x => x.date_from).NotEmpty().NotNull().GreaterThanOrEqualTo((DateTime)SqlDateTime.MinValue).LessThanOrEqualTo(x => x.date_to);
            RuleFor(x => x.date_to).NotEmpty().NotNull().GreaterThanOrEqualTo(x => x.date_from).LessThanOrEqualTo((DateTime)SqlDateTime.MaxValue);
        }
    }
}
