using AutoMapper;
using DatingApp.Contracts.Persistence;
using DatingApp.Dtos.Message;
using DatingApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class MessageController : ControllerBase
 {
 private readonly IUnitOfWork _unitOfWork;
 private readonly IMapper _mapper;

 public MessageController(IUnitOfWork unitOfWork, IMapper mapper)
 {
 _unitOfWork = unitOfWork;
 _mapper = mapper;
 }

 [HttpPost]
 public async Task<IActionResult> Add([FromBody] AddMessageRequest request)
 {
 var message = Message.Create(request.UserId1, request.UserId2, request.Text);
 await _unitOfWork.MessageRepository.AddAsync(message);
 await _unitOfWork.SaveChangesAsync();

 var dto = _mapper.Map<MessageDto>(message);
 return CreatedAtAction(nameof(GetById), new { id = message.Id }, dto);
 }

 [HttpGet("{id}")]
 public async Task<IActionResult> GetById(long id)
 {
 var msg = await _unitOfWork.MessageRepository.GetByIdAsync(id);
 if (msg == null) return NotFound();
 var dto = _mapper.Map<MessageDto>(msg);
 return Ok(dto);
 }
 }
}
