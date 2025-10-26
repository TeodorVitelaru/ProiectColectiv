using FluentValidation;

namespace DatingApp.Dtos.User
{

    /// <summary>
    /// Defines validation rules for the <see cref="GetUserRequest"/>
    /// </summary>
    public class GetUserRequestValidator : AbstractValidator<GetUserRequest>
    {
        /// <summary>
        /// Initialize new instance of the <see cref="GetUserRequestValidator"/>
        /// </summary>
        public GetUserRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        }
    }
}
