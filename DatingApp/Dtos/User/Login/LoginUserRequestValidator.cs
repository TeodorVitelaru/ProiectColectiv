using FluentValidation;

namespace DatingApp.Dtos.User.Login
{
    /// <summary>
    /// Defines validation rules for the <see cref="LoginUserRequest"/>
    /// </summary>
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginUserRequestValidator"/>
        /// </summary>
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        }
    }
}
