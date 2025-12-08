using DatingApp.Dtos.Common;
using DatingApp.Dtos.Message;
using DatingApp.Dtos.User;

namespace DatingApp.Contracts.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetAllMessagesAsync();
        Task<MessageDto> AddMessageAsync (AddMessageRequest request);
        Task<MessageDto> GetMessageAsync (long id);
        Task<MessageDto> EditMessageAsync (EditMessageRequest request);
        Task DeleteMessageAsync (DeleteMessageRequest request);
        Task<IEnumerable<MessageDto>> GetAllMessagesBetween2Users(GetMessagesBetween2UsersRequest request);

        Task<PagedResponse<MessageDto>> GetPaginatedMessagesBetWeen2UsersAsync(GetPaginatedMessagesBetween2UsersRequest request);

        Task<IEnumerable<UserDto>> GetUsersWithMessagesAsync(GetUsersWithMessagesRequest request);
    }
}
