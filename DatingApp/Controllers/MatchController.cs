using DatingApp.Contracts.Services;
using DatingApp.Contracts.Services.HelperService;
using DatingApp.Dtos.Match;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    /// <summary>
    /// Controller for Match operations.
    /// </summary>
    [ApiController]
    [Route("api/matches")]
    [Produces("application/json")]
    [Authorize]
    public class MatchController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        private readonly IMatchService _matchService;
        private readonly IAuthorizationHelperService _authorizationHelperService;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of <see cref="MatchController"/>.
        /// </summary>
        public MatchController(
            ILogger<MatchController> logger,
            IMatchService matchService,
            IAuthorizationHelperService authorizationHelperService,
            IUserService userService)
        {
            _logger = logger;
            _matchService = matchService;
            _authorizationHelperService = authorizationHelperService;
            _userService = userService;

            _logger.LogTrace("{Controller} created", nameof(MatchController));
        }

        /// <summary>
        /// Creates a new match (like).
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MatchResponse>> AddMatchAsync([FromBody] AddMatchRequest request)
        {
            _logger.LogTrace("Add match called");

            var userId = _authorizationHelperService.GetCurrentUserId(HttpContext);
            if (userId <= 0)
            {
                return Unauthorized("User not authenticated.");
            }

            try
            {
                var response = await _matchService.AddMatchAsync(userId, request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid argument: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets all matches for the current user.
        /// </summary>
        [HttpGet("current-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetCurrentUserMatchesAsync()
        {
            _logger.LogTrace("Get current user matches called");

            var userId = _authorizationHelperService.GetCurrentUserId(HttpContext);
            if (userId <= 0)
            {
                return Unauthorized("User not authenticated.");
            }

            var matches = await _matchService.GetUserMatchesAsync(userId);
            return Ok(matches);
        }

        /// <summary>
        /// Gets only mutual matches for the current user.
        /// </summary>
        [HttpGet("current-user/mutual")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetCurrentUserMutualMatchesAsync()
        {
            _logger.LogTrace("Get current user mutual matches called");

            var userId = _authorizationHelperService.GetCurrentUserId(HttpContext);
            if (userId <= 0)
            {
                return Unauthorized("User not authenticated.");
            }

            var matches = await _matchService.GetMutualMatchesAsync(userId);
            return Ok(matches);
        }

        /// <summary>
        /// Checks if the current user has matched with a specific user.
        /// </summary>
        [HttpGet("check/{otherUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<dynamic>> CheckMatchAsync([FromRoute] long otherUserId)
        {
            _logger.LogTrace("Check match called");

            var userId = _authorizationHelperService.GetCurrentUserId(HttpContext);
            if (userId <= 0)
            {
                return Unauthorized("User not authenticated.");
            }

            var isMatched = await _matchService.AreMatchedAsync(userId, otherUserId);
            var isMutual = await _matchService.IsMutualMatchAsync(userId, otherUserId);

            return Ok(new { isMatched, isMutual });
        }

        /// <summary>
        /// Deletes a match (unlike).
        /// </summary>
        [HttpDelete("{matchId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMatchAsync([FromRoute] long matchId)
        {
            _logger.LogTrace("Delete match called for ID {MatchId}", matchId);

            var userId = _authorizationHelperService.GetCurrentUserId(HttpContext);
            if (userId <= 0)
            {
                return Unauthorized("User not authenticated.");
            }

            var result = await _matchService.DeleteMatchAsync(matchId);
            if (!result)
            {
                return NotFound("Match not found.");
            }

            return NoContent();
        }

        /// <summary>
        /// Adds a dislike for a user (swipe left).
        /// </summary>
        [HttpPost("dislike/{dislikedUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MatchResponse>> AddDislikeAsync(long dislikedUserId)
        {
            _logger.LogTrace("Add dislike called for user {DislikedUserId}", dislikedUserId);

            var userId = _authorizationHelperService.GetCurrentUserId(HttpContext);
            if (userId <= 0)
            {
                return Unauthorized("User not authenticated.");
            }

            try
            {
                var response = await _matchService.AddDislikeAsync(userId, dislikedUserId);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid argument: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a random unmatched user for the current user (for swiping).
        /// </summary>
        [HttpGet("random-unmatched")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRandomUnmatchedUserAsync()
        {
            _logger.LogTrace("Get random unmatched user called");

            var userId = _authorizationHelperService.GetCurrentUserId(HttpContext);
            if (userId <= 0)
            {
                return Unauthorized("User not authenticated.");
            }

            var users = await _matchService.GetRandomUnmatchedUsersAsync(userId, 1);
            var user = users.FirstOrDefault();

            if (user is null)
            {
                return NotFound("No more users available to match with.");
            }

            // Map to DTO with images
            var photos = user.Images?.Select(i => new { i.Id, ImageUrl = Convert.ToBase64String(i.ImageData) }).ToList();
            
            var userDto = new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                Age = user.Age ?? 0,
                Height = user.Height ?? 0,
                Location = user.City ?? string.Empty,
                Bio = user.Bio ?? string.Empty,
                Photos = (object?)(photos != null ? photos : new object[] { })
            };

            return Ok(userDto);
        }
    }
}
