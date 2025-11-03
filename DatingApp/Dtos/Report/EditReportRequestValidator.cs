using FluentValidation;

namespace DatingApp.Dtos.Report
{
    public class EditReportRequestValidator : AbstractValidator<EditReportRequest>
    {
        public EditReportRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Reason).NotEmpty();
        }
    }
}


