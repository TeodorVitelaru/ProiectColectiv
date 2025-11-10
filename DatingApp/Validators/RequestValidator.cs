using DatingApp.Contracts.Validators;
using FluentValidation;

namespace DatingApp.Validators
{
    public class RequestValidator : IRequestValidator
    {
        private readonly ILogger<RequestValidator> _logger;
        private readonly IValidationFactory _validatorFactory;

        public RequestValidator(ILogger<RequestValidator> logger, IValidationFactory validatorFactory)
        {
            _logger = logger;
            _validatorFactory = validatorFactory;
        }

        public void Validate<T>(T request) where T : class
        {
            IValidator<T> validator = _validatorFactory.CreateInstance<T>();

            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (ValidationException ex)
            {
                _logger.LogDebug("{Validator} - {Message}", validator.GetType().Name, ex.Message);
                throw;
            }
        }
    }
}
