using FluentValidation;
using DatingApp.Dtos.ValidationSettings;

namespace DatingApp.Dtos.User
{
    /// <summary>
    /// Defines validation rules for the <see cref="AddUserRequest"/>
    /// </summary>
    public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserRequestValidator"/>
        /// </summary>
        public AddUserRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50).Matches(NameValidationSettings.NameRegex)
                .WithMessage($"The name can only contain letters and spaces.");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50).Matches(NameValidationSettings.NameRegex)
                .WithMessage($"The last name can only contain letters and spaces.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().Matches(UserEmailValidationSettings.UserEmailRegex)
                .WithMessage($"Email not in correct format.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }
}
