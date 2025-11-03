using FluentValidation;

namespace DatingApp.Dtos.Review
{
    public class AddReviewRequestValidator : AbstractValidator<AddReviewRequest>
    {
        public AddReviewRequestValidator()
        {
            RuleFor(x => x.ReviewerId).GreaterThan(0);
            RuleFor(x => x.RevieweeId).GreaterThan(0);
            RuleFor(x => x.ReviewerId).NotEqual(x => x.RevieweeId);
            RuleFor(x => x.Rating).InclusiveBetween(1, 5);
            RuleFor(x => x.Comment).NotEmpty();
        }
    }
}