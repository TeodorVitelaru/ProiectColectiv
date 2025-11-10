using FluentValidation;

namespace DatingApp.Contracts.Validators
{
    public interface IValidationFactory
    {
        IValidator<T> CreateInstance<T>();
    }
}
