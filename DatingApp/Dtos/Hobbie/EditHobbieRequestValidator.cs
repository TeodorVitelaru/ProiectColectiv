using FluentValidation;

namespace DatingApp.Dtos.Hobbie
{
    public class EditHobbieRequestValidator : AbstractValidator<EditHobbieRequest>
    {
        public EditHobbieRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.HobbieName).NotEmpty();
        }
    }
}
