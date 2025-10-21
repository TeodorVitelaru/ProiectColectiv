using FluentValidation;

namespace DatingApp.Dtos.User
{
    /// <summary>
    /// Defines validation rules for the <see cref="DeleteUserRequest"/>
    /// </summary>
    public class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
    {
        /// <summary>
        /// Initialize new instance of <see cref="DeleteUserRequestValidator"/>
        /// </summary>
        public DeleteUserRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        }
    }
}
