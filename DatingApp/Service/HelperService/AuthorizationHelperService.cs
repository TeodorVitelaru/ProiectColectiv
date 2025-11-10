using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services.HelperService;
using System.Security.Claims;

namespace DatingApp.Service.HelperService
{
    public class AuthorizationHelperService : IAuthorizationHelperService
    {
        private readonly ILogger<AuthorizationHelperService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationHelperService"/>.
        /// </summary>
        /// <param name="logger">Writes log messages.</param>
        /// <param name="unitOfWork">Used for database migrations.</param>
        public AuthorizationHelperService(ILogger<AuthorizationHelperService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<bool> IsUserAllowedToExecuteMethod(IEnumerable<Claim> claims, long id)
        {
            _logger.LogTrace("Validate authorization called.");

            var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            var loginUserId = claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (role == "User")
            {
                var user = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Id == long.Parse(loginUserId));
                loginUserId = user.Id.ToString();
            }

            if (loginUserId == id.ToString() || role == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
