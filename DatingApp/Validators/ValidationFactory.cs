using DatingApp.Contracts.Validators;
using FluentValidation;

namespace DatingApp.Validators
{
    public class ValidationFactory : IValidationFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        public ValidationFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public IValidator<T> CreateInstance<T>()
        {
            var validator = _serviceProvider.GetService(typeof(IValidator<T>));
            if (validator != null)
            {
                return (IValidator<T>)validator;
            }

            return null!;
        }
    }
}
