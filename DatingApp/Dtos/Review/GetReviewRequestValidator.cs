using FluentValidation;

namespace DatingApp.Dtos.Review
{
    public class GetReviewRequestValidator : AbstractValidator<GetReviewRequest>
    {
        public GetReviewRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
