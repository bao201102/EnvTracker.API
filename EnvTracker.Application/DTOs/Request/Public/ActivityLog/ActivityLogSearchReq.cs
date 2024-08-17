using EnvTracker.Application.Common;
using EnvTracker.Application.Utilities;
using FluentValidation;

namespace EnvTracker.Application.DTOs.Request.Public.ActivityLog
{
    public class ActivityLogSearchReq : BasePgDto
    {
        public string? keysearch { get; set; }
        public int page_size { get; set; } = ModelConfig.PageSizeDefaultValue;
        public int page_index { get; set; } = 0;
    }

    public class ActivityLogSearchReqValidator : AbstractValidator<ActivityLogSearchReq>
    {
        public ActivityLogSearchReqValidator()
        {
            RuleFor(x => x.page_size).GreaterThanOrEqualTo(-1).LessThanOrEqualTo(ModelConfig.PageSizeMaxValue);
            RuleFor(x => x.page_size).GreaterThan(0).When(c => c.page_index >= 0);
            RuleFor(x => x.page_index).GreaterThanOrEqualTo(-1);
            RuleFor(x => x.page_index).GreaterThanOrEqualTo(0).When(c => c.page_size > 0);
        }
    }
}
