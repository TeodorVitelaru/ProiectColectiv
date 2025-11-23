using DatingApp.Dtos.Report;
using FluentValidation;

namespace DatingApp.Dtos.Hobbie
{
    public class AddHobbieRequestValidator : AbstractValidator<AddHobbieRequest>
    {
        public AddHobbieRequestValidator()
        {
            RuleFor(x => x.HobbieName).NotEmpty();
        }
    }
}
