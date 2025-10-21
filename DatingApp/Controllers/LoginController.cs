using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Dtos.User.Login;
using DatingApp.Contracts.Services;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/login")]
    [Produces("application/json")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginService;

        public LoginController(ILogger<LoginController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
            _logger.LogTrace("{Controller} called", nameof(LoginController));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TokenDto>> Login(LoginUserRequest request)
        {
            _logger.LogTrace("Login called");

            TokenDto token = await _loginService.GetLoginToken(request);

            return token;
        }
    }
}
