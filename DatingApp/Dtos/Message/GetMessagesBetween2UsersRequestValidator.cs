using DatingApp.Dtos.Message;
using FluentValidation;

namespace DatingApp.Dtos.Message
{
    /// <summary>
    /// Defines validation rules for the <see cref="GetMessagesBetween2UsersRequest"/>
    /// </summary>
    public class GetMessagesBetween2UsersRequestValidator : AbstractValidator<GetMessagesBetween2UsersRequest>
    {
        public GetMessagesBetween2UsersRequestValidator() 
        {
            RuleFor(x => x.FirstUserId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.SecondUserId).NotEmpty().GreaterThan(0);
        }
    }
}
