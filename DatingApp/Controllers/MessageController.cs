using AutoMapper;
using DatingApp.Contracts.Services;
using DatingApp.Dtos.Message;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 [Produces("application/json")]
 public class MessageController : ControllerBase
 {
 private readonly IMessageService _messageService;
 private readonly IMapper _mapper;

 public MessageController(IMessageService messageService, IMapper mapper)
 {
 _messageService = messageService;
 _mapper = mapper;
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
 }
}
