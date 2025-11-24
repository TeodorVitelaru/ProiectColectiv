using DatingApp.Domain.Entities;

namespace DatingApp.Contracts.Persistence
{
    public interface IMessageRepository : IRepository<Message, long>
    {
        Task<(List<Message> messages, int totalCount)> GetPaginatedMessagesBetweenTwoUsersAsync(
            long senderId,
            long recipientId,
            int pageNumber,
            int pageSize);    }
}