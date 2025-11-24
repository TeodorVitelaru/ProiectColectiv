using AutoMapper;
using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Contracts.Validators;
using DatingApp.Dtos.Message;
using DatingApp.Domain.Entities;
using DatingApp.Exceptions;
using System.Linq;
using DatingApp.Dtos.Common;

namespace DatingApp.Service
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestValidator _requestValidator;
        private readonly IMapper _mapper;

        public MessageService(ILogger<MessageService> logger, IUnitOfWork unitOfWork,
            IRequestValidator requestValidator, IMapper mapper)
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
            var sender = await _unitOfWork.UserRepository.GetByIdAsync(request.SenderId);
            if (sender == null) throw new NotFoundException("User", request.SenderId);
            var recipient = await _unitOfWork.UserRepository.GetByIdAsync(request.RecipientId);
            if (recipient == null) throw new NotFoundException("User", request.RecipientId);

            var message =
                await _unitOfWork.MessageRepository.AddAsync(Message.Create(request.SenderId, request.RecipientId,
                    request.Text));
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MessageDto>(message);
        }

        public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync()
        {
            // include sender and recipient
            var messages = await _unitOfWork.MessageRepository.FindAsync(m => true, m => m.Sender, m => m.Recipient);
            return messages.Select(m => _mapper.Map<MessageDto>(m));
        }

        public async Task<MessageDto> GetMessageAsync(long id)
        {
            var msg = await _unitOfWork.MessageRepository.FindFirstOrDefaultAsync(m => m.Id == id, m => m.Sender,
                          m => m.Recipient)
                      ?? throw new NotFoundException("Message", id);
            return _mapper.Map<MessageDto>(msg);
        }

        public async Task<MessageDto> EditMessageAsync(EditMessageRequest request)
        {
            _requestValidator.Validate(request);
            var existing = await _unitOfWork.MessageRepository.GetByIdAsync(request.Id) ??
                           throw new NotFoundException("Message", request.Id);
            existing.UpdateText(request.Text);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MessageDto>(existing);
        }

        public async Task DeleteMessageAsync(DeleteMessageRequest request)
        {
            var existing = await _unitOfWork.MessageRepository.GetByIdAsync(request.Id) ??
                           throw new NotFoundException("Message", request.Id);
            _unitOfWork.MessageRepository.Remove(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessageDto>> GetAllMessagesBetween2Users(GetMessagesBetween2UsersRequest request)
        {
            _logger.LogTrace("Get messages between 2 users called.");

            _requestValidator.Validate(request);

            User firstUser = await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Id == request.FirstUserId)
                             ?? throw new NotFoundException(nameof(User), request.FirstUserId);

            User secondUser =
                await _unitOfWork.UserRepository.FindFirstOrDefaultAsync(u => u.Id == request.SecondUserId)
                ?? throw new NotFoundException(nameof(User), request.SecondUserId);

            IEnumerable<Message> messagesBetweenUsers = await _unitOfWork.MessageRepository
                .FindAsync(m => (m.SenderId == firstUser.Id && m.RecipientId == secondUser.Id) ||
                                (m.SenderId == secondUser.Id && m.RecipientId == firstUser.Id));

            if (!messagesBetweenUsers.Any())
            {
                throw new NotFoundException("There are no messages.");
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messagesBetweenUsers);
        }

        public async Task<PagedResponse<MessageDto>> GetPaginatedMessagesBetWeen2UsersAsync(int senderId,
            long recipientId, GetPaginatedMessagesBetween2UsersRequest request)
        {
            {
                _logger.LogTrace("Get paginated msg called");

                _requestValidator.Validate(request);

                var (messages, totalCount) =
                    await _unitOfWork.MessageRepository.GetPaginatedMessagesBetweenTwoUsersAsync(
                        senderId,
                        recipientId,
                        request.PageNumber,
                        request.PageSize);

                var messageDtos = _mapper.Map<List<MessageDto>>(messages);

                return new PagedResponse<MessageDto>(
                    messageDtos
                    , totalCount
                    , request.PageNumber
                    , request.PageSize);
            }
        }
    }
}