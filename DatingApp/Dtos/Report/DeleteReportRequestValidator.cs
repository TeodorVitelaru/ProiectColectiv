using FluentValidation;

namespace DatingApp.Dtos.Report
{
    public class DeleteReportRequestValidator : AbstractValidator<DeleteReportRequest>
    {
        public DeleteReportRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}


