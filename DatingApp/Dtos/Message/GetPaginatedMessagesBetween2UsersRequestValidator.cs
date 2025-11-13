using FluentValidation;

namespace DatingApp.Dtos.Message;

public class GetPaginatedMessagesBetween2UsersRequestValidator: AbstractValidator<GetPaginatedMessagesBetween2UsersRequest>
{
    public GetPaginatedMessagesBetween2UsersRequestValidator()
    {
        RuleFor(x => x.PageNumber).NotEmpty().WithMessage("PageNumber is required.")
            .GreaterThan(0).WithMessage("PageNumber must be a positive number");
        RuleFor(x => x.PageSize).NotEmpty().WithMessage("PageSize is required")
            .GreaterThan(0).WithMessage("PageSize must be a positive number")
            .LessThanOrEqualTo(100).WithMessage("PageSize cannot be greater than 100");
    }
}