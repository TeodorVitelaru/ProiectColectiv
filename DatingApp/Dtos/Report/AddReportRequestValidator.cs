using FluentValidation;

namespace DatingApp.Dtos.Report
{
    public class AddReportRequestValidator : AbstractValidator<AddReportRequest>
    {
        public AddReportRequestValidator()
        {
            RuleFor(x => x.ReporterId).GreaterThan(0);
            RuleFor(x => x.ReportedUserId).GreaterThan(0);
            RuleFor(x => x.ReporterId).NotEqual(x => x.ReportedUserId);
            RuleFor(x => x.Reason).NotEmpty();
        }
    }
}


