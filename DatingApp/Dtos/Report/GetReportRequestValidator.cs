using FluentValidation;

namespace DatingApp.Dtos.Report
{
    public class GetReportRequestValidator : AbstractValidator<GetReportRequest>
    {
        public GetReportRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}


