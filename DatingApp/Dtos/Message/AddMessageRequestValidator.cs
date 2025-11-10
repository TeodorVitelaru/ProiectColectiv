using DatingApp.Dtos.User;
using FluentValidation;

namespace DatingApp.Dtos.Message
{
    public class AddMessageRequestValidator : AbstractValidator<AddMessageRequest>
    {
        public AddMessageRequestValidator() 
        {
            RuleFor(x => x.RecipientId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.SenderId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Text).NotEmpty().WithMessage("Message can't be empty");
        }
    }
}
