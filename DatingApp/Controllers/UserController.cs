using DatingApp.Contracts.Services;
using DatingApp.Contracts.Services.HelperService;
using DatingApp.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IAuthorizationHelperService _authorizationHelperService;

        /// <summary>
        /// Initializes a new instance of <see cref="UserController"/>
        /// </summary>
        public UserController(ILogger<UserController> logger, IUserService userService, IAuthorizationHelperService authorizationHelperService)
        {
            _logger = logger;
            _userService = userService;
            _authorizationHelperService = authorizationHelperService;

            _logger.LogTrace("{Controller} created", nameof(UserController));
        }

        /// <summary>
        /// Asynchronously gets all <see cref="UserDto"/> objects.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            _logger.LogTrace("Get all users called");

            IEnumerable<UserDto> response = await _userService.GetAllUsersAsync();

            return response.ToList();
        }

        /// <summary>
        /// Asynchronously gets <see cref="UserDto"/> object for provided <paramref name="userId"/>.
        /// </summary>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,TA,Interviewer")]
        public async Task<ActionResult<UserDto>> GetUserAsync(long userId)
        {
            _logger.LogTrace($"Get user with ID {userId} called");

            UserDto response = await _userService.GetUserAsync(new GetUserRequest { Id = userId });

            return response;
        }

        /// <summary>
        /// Asynchronously creates <see cref="UserDto"/> object for provided <paramref name="request"/>.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> CreateUserAsync(AddUserRequest request)
        {
            _logger.LogTrace("Create user called");

            UserDto response = await _userService.AddUserAsync(request);

            return response;
        }

        /// <summary>
        /// Asynchronously updates <see cref="UserDto"/> object for provided <paramref name="userId"/> and <paramref name="request"/>.
        /// </summary>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> UpdateUserAsync(long userId, EditUserRequest request)
        {
            _logger.LogTrace($"Update user with ID {userId} called");

            if (userId != request.Id)
            {
                return BadRequest();
            }

            UserDto response = await _userService.EditUserAsync(request);

            return response;
        }

        /// <summary>
        /// Asynchronously deletes <see cref="UserDto"/> object for provided <paramref name="userId"/>.
        /// </summary>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUserAsync(long userId)
        {
            _logger.LogTrace($"Delete user with ID {userId} called");

            await _userService.DeleteUserAsync(new DeleteUserRequest { Id = userId });

            return NoContent();
        }
    }
}
