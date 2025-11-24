using DatingApp.Contracts.Services;
using DatingApp.Contracts.Services.HelperService;
using DatingApp.Dtos.Hobbie;
using DatingApp.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/hobbies")]
    [Produces("application/json")]
    public class HobbieController : ControllerBase
    {
        private readonly ILogger<HobbieController> _logger;
        private readonly IHobbieService _hobbieService;

        public HobbieController(
            ILogger<HobbieController> logger,
            IHobbieService hobbieService)
        {
            _logger = logger;
            _hobbieService = hobbieService;

            _logger.LogTrace("{Controller} created", nameof(HobbieController));
        }

        /// <summary>
        /// Asynchronously gets all <see cref="HobbieDto"/> objects.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<HobbieDto>>> GetAllHobbiesAsync()
        {
            _logger.LogTrace("Get all hobbies called");

            IEnumerable<HobbieDto> response = await _hobbieService.GetAllHobbiesAsync();
            return response.ToList();
        }

        /// <summary>
        /// Asynchronously gets <see cref="HobbieDto"/> for provided <paramref name="hobbieId"/>.
        /// </summary>
        [HttpGet("{hobbieId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HobbieDto>> GetHobbieAsync(long hobbieId)
        {
            _logger.LogTrace($"Get hobbie with ID {hobbieId} called");

            HobbieDto response =
                await _hobbieService.GetHobbieAsync(new GetHobbieRequest { HobbieId = hobbieId });

            return response;
        }

        /// <summary>
        /// Asynchronously creates a new <see cref="HobbieDto"/>.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HobbieDto>> CreateHobbieAsync(AddHobbieRequest request)
        {
            _logger.LogTrace("Create hobbie called");

            HobbieDto response = await _hobbieService.AddHobbieAsync(request);

            return response;
        }

        /// <summary>
        /// Asynchronously updates <see cref="HobbieDto"/> object for provided <paramref name="hobbieId"/>.
        /// </summary>
        [HttpPut("{hobbieId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HobbieDto>> UpdateHobbieAsync(long hobbieId, EditHobbieRequest request)
        {
            _logger.LogTrace($"Update hobbie with ID {hobbieId} called");

            if (hobbieId != request.Id)
                return BadRequest();

            HobbieDto response = await _hobbieService.EditHobbieAsync(request);
            return response;
        }

        /// <summary>
        /// Asynchronously deletes the hobbie with provided <paramref name="hobbieId"/>.
        /// </summary>
        [HttpDelete("{hobbieId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteHobbieAsync(long hobbieId)
        {
            _logger.LogTrace($"Delete hobbie with ID {hobbieId} called");

            await _hobbieService.DeleteReviewAsync(new DeleteHobbieRequest { Id = hobbieId });

            return NoContent();
        }

        /// <summary>
        /// Asynchronously gets all hobbies for provided user.
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<HobbieDto>>> GetHobbiesForUserAsync(long userId)
        {
            _logger.LogTrace($"Get hobbies for user with ID {userId} called");

            var response = await _hobbieService
                .GetAllHobbiesForUserAsync(new GetAllHobbiesForUserRequest { UserId = userId });

            return response.ToList();
        }

        /// <summary>
        /// Asynchronously adds a hobbie to a user.
        /// </summary>
        [HttpPost("assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> AddHobbieToUserAsync(AddHobbieToUserRequest request)
        {
            _logger.LogTrace("Add hobbie to user called");

            var response = await _hobbieService.AddHobbieToUserAsync(request);

            return response;
        }
    }
}
