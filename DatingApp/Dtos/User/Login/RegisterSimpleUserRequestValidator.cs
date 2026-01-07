using FluentValidation;

namespace DatingApp.Dtos.User.Login
{
    /// <summary>
    /// Validator for RegisterSimpleUserRequest.
    /// </summary>
    public class RegisterSimpleUserRequestValidator : AbstractValidator<RegisterSimpleUserRequest>
    {
        /// <summary>
        /// Initializes a new instance of RegisterSimpleUserRequestValidator.
        /// </summary>
        public RegisterSimpleUserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be valid.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");
        }
    }
}
