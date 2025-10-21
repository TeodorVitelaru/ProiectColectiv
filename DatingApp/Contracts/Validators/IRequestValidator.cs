namespace DatingApp.Contracts.Validators
{
    public interface IRequestValidator
    {
        void Validate<T>(T request) where T : class;
    }
}
