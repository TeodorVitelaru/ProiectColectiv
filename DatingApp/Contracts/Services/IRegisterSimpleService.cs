using DatingApp.Dtos.User.Login;

namespace DatingApp.Contracts.Services
{
    /// <summary>
    /// Service for simple user registration with JWT token.
    /// </summary>
    public interface IRegisterSimpleService
    {
        /// <summary>
        /// Registers a new user with basic info and returns JWT token.
        /// </summary>
        /// <param name="request">Registration request with email and password.</param>
        /// <returns>TokenDto with JWT token.</returns>
        Task<TokenDto> RegisterUserAsync(RegisterSimpleUserRequest request);
    }
}
