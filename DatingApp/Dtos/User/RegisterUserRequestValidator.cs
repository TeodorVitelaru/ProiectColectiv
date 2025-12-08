using FluentValidation;

namespace DatingApp.Dtos.User
{
    /// <summary>
    /// Validator for <see cref="RegisterUserRequest"/>.
    /// </summary>
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RegisterUserRequestValidator"/> class.
        /// </summary>
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Age)
                .InclusiveBetween(18, 100).WithMessage("Age must be between 18 and 100.");

            RuleFor(x => x.Height)
                .InclusiveBetween(100, 250).WithMessage("Height must be between 100 and 250 cm.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid gender value.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(100).WithMessage("Location cannot exceed 100 characters.");

            RuleFor(x => x.Languages)
                .NotEmpty().WithMessage("At least one language is required.")
                .Must(x => x.Count > 0).WithMessage("At least one language is required.");

            RuleFor(x => x.Photos)
                .NotEmpty().WithMessage("At least 2 photos are required.")
                .Must(x => x != null && x.Count >= 2).WithMessage("At least 2 photos are required.")
                .Must(x => x == null || x.Count <= 6).WithMessage("Maximum 6 photos allowed.");

            RuleFor(x => x.Hobbies)
                .NotEmpty().WithMessage("At least one hobby/interest is required.")
                .Must(x => x.Count >= 1 && x.Count <= 5).WithMessage("Please select between 1 and 5 hobbies/interests.");

            RuleFor(x => x.Bio)
                .NotEmpty().WithMessage("Bio is required.")
                .MinimumLength(10).WithMessage("Bio must be at least 10 characters.")
                .MaximumLength(500).WithMessage("Bio cannot exceed 500 characters.");

            RuleFor(x => x.RelationshipGoal)
                .IsInEnum().WithMessage("Invalid relationship goal value.");

            RuleFor(x => x.SexualOrientation)
                .IsInEnum().WithMessage("Invalid sexual orientation value.");

            RuleFor(x => x.PreferredAgeMin)
                .InclusiveBetween(18, 100).WithMessage("Minimum preferred age must be between 18 and 100.");

            RuleFor(x => x.PreferredAgeMax)
                .InclusiveBetween(18, 120).WithMessage("Maximum preferred age must be between 18 and 120.")
                .GreaterThanOrEqualTo(x => x.PreferredAgeMin).WithMessage("Maximum age must be greater than or equal to minimum age.");
        }
    }
}

