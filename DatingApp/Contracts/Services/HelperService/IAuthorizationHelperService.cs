using System.Security.Claims;

namespace DatingApp.Contracts.Services.HelperService
{
    /// <summary>
    /// Provides management of operations for id authorization.
    /// </summary>
    public interface IAuthorizationHelperService
    {
        /// <summary>
        /// Asynchronously secures authorization for method for provided <paramref name="claims"/> and <paramref name="id"/>.
        /// </summary>
        /// <param name="claims">Information about logged in user.</param>
        /// <param name="id">Identifier from request.</param>
        Task<bool> IsUserAllowedToExecuteMethod(IEnumerable<Claim> claims, long id);
    }
}
