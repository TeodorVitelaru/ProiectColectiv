using FluentValidation;

namespace DatingApp.Dtos.Hobbie
{
    public class AddHobbieToUserRequestValidator : AbstractValidator<AddHobbieToUserRequest>
    {
        public AddHobbieToUserRequestValidator()
        {
            RuleFor(x => x.HobbieId).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
