using AutoMapper;
using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Contracts.Validators;
using DatingApp.Dtos.Message;
using DatingApp.Domain.Entities;
using DatingApp.Exceptions;

namespace DatingApp.Service
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestValidator _requestValidator;
        private readonly IMapper _mapper;

        public MessageService(ILogger<MessageService> logger, IUnitOfWork unitOfWork, IRequestValidator requestValidator, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _requestValidator = requestValidator;
            _mapper = mapper;
        }

        public async Task<MessageDto> AddMessageAsync(AddMessageRequest request)
        {
            _logger.LogTrace("Add message called");

            _requestValidator.Validate(request);

            // validate users exist
            var user1 = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId1);
            if (user1 == null) throw new NotFoundException("User", request.UserId1);
            var user2 = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId2);
            if (user2 == null) throw new NotFoundException("User", request.UserId2);

            var message = await _unitOfWork.MessageRepository.AddAsync(Message.Create(request.UserId1, request.UserId2, request.Text));
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MessageDto>(message);
        }

        public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync()
        {
            var messages = await _unitOfWork.MessageRepository.GetAllAsync();
            return messages.Select(m => _mapper.Map<MessageDto>(m));
        }

        public async Task<MessageDto> GetMessageAsync(long id)
        {
            var msg = await _unitOfWork.MessageRepository.GetByIdAsync(id) ?? throw new NotFoundException("Message", id);
            return _mapper.Map<MessageDto>(msg);
        }

        public async Task<MessageDto> EditMessageAsync(EditMessageRequest request)
        {
            _requestValidator.Validate(request);
            var existing = await _unitOfWork.MessageRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException("Message", request.Id);
            existing.UpdateText(request.Text);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MessageDto>(existing);
        }

        public async Task DeleteMessageAsync(DeleteMessageRequest request)
        {
            var existing = await _unitOfWork.MessageRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException("Message", request.Id);
            _unitOfWork.MessageRepository.Remove(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
