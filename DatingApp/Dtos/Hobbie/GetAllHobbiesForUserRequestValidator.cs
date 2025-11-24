using DatingApp.Dtos.Report;
using FluentValidation;

namespace DatingApp.Dtos.Hobbie
{
    public class GetAllHobbiesForUserRequestValidator : AbstractValidator<GetAllHobbiesForUserRequest>
    {
        public GetAllHobbiesForUserRequestValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
