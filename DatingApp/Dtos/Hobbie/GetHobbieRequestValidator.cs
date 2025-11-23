using FluentValidation;

namespace DatingApp.Dtos.Hobbie
{
    public class GetHobbieRequestValidator : AbstractValidator<GetHobbieRequest>
    {
        public GetHobbieRequestValidator()
        {
            RuleFor(x => x.HobbieId).GreaterThan(0);
        }
    }
}
