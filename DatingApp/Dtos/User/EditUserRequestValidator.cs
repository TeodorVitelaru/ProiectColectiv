using DatingApp.Dtos.ValidationSettings;
using FluentValidation;

namespace DatingApp.Dtos.User
{
    /// <summary>
    /// Defines validation rules for the <see cref="EditUserRequest"/>
    /// </summary>
    public class EditUserRequestValidator : AbstractValidator<EditUserRequest>
    {
        /// <summary>
        /// Initialize new instance of the <see cref="EditUserRequestValidator"/>
        /// </summary>
        public EditUserRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50).Matches(NameValidationSettings.NameRegex)
                .WithMessage($"The name can only contain letters and spaces.");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50).Matches(NameValidationSettings.NameRegex)
                .WithMessage($"The last name can only contain letters and spaces.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().Matches(UserEmailValidationSettings.UserEmailRegex)
                .WithMessage($"Email not in correct format."); ;
        }
    }
}
