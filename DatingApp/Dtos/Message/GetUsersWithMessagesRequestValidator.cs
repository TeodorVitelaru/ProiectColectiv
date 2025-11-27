using FluentValidation;

namespace DatingApp.Dtos.Message
{
    /// <summary>
    /// Validator for <see cref="GetUsersWithMessagesRequest"/>.
    /// </summary>
    public class GetUsersWithMessagesRequestValidator : AbstractValidator<GetUsersWithMessagesRequest>
    {
        public GetUsersWithMessagesRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}

