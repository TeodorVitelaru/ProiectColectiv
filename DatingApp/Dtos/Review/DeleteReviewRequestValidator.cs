using FluentValidation;

namespace DatingApp.Dtos.Review
{
    public class DeleteReviewRequestValidator : AbstractValidator<DeleteReviewRequest>
    {
        public DeleteReviewRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}