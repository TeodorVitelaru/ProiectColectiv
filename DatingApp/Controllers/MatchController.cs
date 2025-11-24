using AutoMapper;
using DatingApp.Contracts.Services;
using DatingApp.Dtos.Match;
using DatingApp.Dtos.Match;
using DatingApp.Dtos.User;
using DatingApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly IMapper _mapper;

        public MatchController(IMatchService matchService, IMapper mapper)
        {
            _mapper = mapper;
            _matchService = matchService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll()
        {
            var matches = await _matchService.GetAllMatchesAsync();
            if (matches == null || !matches.Any()) return NotFound();
            return Ok(matches);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _matchService.GetMatchAsync(id);
            return Ok(dto);
        }

        [HttpGet("user/{userId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetByUserId(long userId)
        {
            var matches = await _matchService.GetAllMatchesOfUser(userId);
            if (matches == null || !matches.Any()) return NotFound();
            return Ok(matches);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AddMatchRequest request)
        {
            var dto = await _matchService.AddMatchAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            await _matchService.DeleteMatchAsync(new DeleteMatchRequest { Id = id });
            return NoContent();
        }
    }
}
