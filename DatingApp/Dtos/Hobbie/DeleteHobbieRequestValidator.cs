using FluentValidation;

namespace DatingApp.Dtos.Hobbie
{
    public class DeleteHobbieRequestValidator : AbstractValidator<DeleteHobbieRequest>
    {
        public DeleteHobbieRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
