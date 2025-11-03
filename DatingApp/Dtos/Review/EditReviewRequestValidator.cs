using FluentValidation;

namespace DatingApp.Dtos.Review
{
    public class EditReviewRequestValidator : AbstractValidator<EditReviewRequest>
    {
        public EditReviewRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Rating).InclusiveBetween(1, 5);
            RuleFor(x => x.Comment).NotEmpty();
        }
    }
}