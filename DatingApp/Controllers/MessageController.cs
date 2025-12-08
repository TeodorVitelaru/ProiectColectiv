using AutoMapper;
using DatingApp.Contracts.Services;
using DatingApp.Contracts.Services.HelperService;
using DatingApp.Dtos.Common;
using DatingApp.Dtos.Message;
using DatingApp.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/messages")]
    [Produces("application/json")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly IAuthorizationHelperService _authorizationHelperService;


        public MessageController(IMessageService messageService, IMapper mapper, IAuthorizationHelperService authorizationHelperService)
        {
            _messageService = messageService;
            _mapper = mapper;
            _authorizationHelperService = authorizationHelperService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetAll()
        {
            var messages = await _messageService.GetAllMessagesAsync();
            if (messages == null || !messages.Any()) return NotFound();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _messageService.GetMessageAsync(id);
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AddMessageRequest request)
        {
            var dto = await _messageService.AddMessageAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] EditMessageRequest request)
        {
            if (id != request.Id) return BadRequest();
            var dto = await _messageService.EditMessageAsync(request);
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            await _messageService.DeleteMessageAsync(new DeleteMessageRequest { Id = id });
            return NoContent();
        }

        [HttpGet("users/{firstUserId}/users/{secondUserId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IEnumerable<MessageDto>> GetMessagesBetween2UsersAsync(long firstUserId, long secondUserId)
        {
            IEnumerable<MessageDto> messages = await _messageService
                .GetAllMessagesBetween2Users(new GetMessagesBetween2UsersRequest
                    { FirstUserId = firstUserId, SecondUserId = secondUserId });

            return messages;
        }

        [HttpGet("users/{recipientId}/paginated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<PagedResponse<MessageDto>>> GetPaginatedMessagesAsync(
            long recipientId, [FromQuery] GetPaginatedMessagesBetween2UsersRequest request)
        {
            var senderId = _authorizationHelperService.GetCurrentUserId(HttpContext);

            var response = await _messageService.GetPaginatedMessagesBetWeen2UsersAsync(senderId, recipientId, request);

            return Ok(response);
        }

        [HttpGet("users/{userId}/conversations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersWithMessagesAsync(long userId)
        {
            var result = await _messageService.GetUsersWithMessagesAsync(
                new GetUsersWithMessagesRequest { UserId = userId });

            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}