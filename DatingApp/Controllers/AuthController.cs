using DatingApp.Contracts.Services;
using DatingApp.Dtos.User.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IRegisterSimpleService _registerSimpleService;

        /// <summary>
        /// Initializes a new instance of AuthController.
        /// </summary>
        public AuthController(ILogger<AuthController> logger, IRegisterSimpleService registerSimpleService)
        {
            _logger = logger;
            _registerSimpleService = registerSimpleService;
            _logger.LogTrace("{Controller} called", nameof(AuthController));
        }

        /// <summary>
        /// Registers a new user with basic information and returns JWT token.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> Register(RegisterSimpleUserRequest request)
        {
            _logger.LogTrace("Register called");

            TokenDto token = await _registerSimpleService.RegisterUserAsync(request);

            return token;
        }
    }
}
