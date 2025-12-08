using DatingApp.Domain.Entities;
using System.Collections.Generic;

namespace DatingApp.Contracts.Persistence
{
    public interface IMessageRepository : IRepository<Message, long>
    {
        Task<(List<Message> messages, int totalCount)> GetPaginatedMessagesBetweenTwoUsersAsync(
            long senderId,
            long recipientId,
            int pageNumber,
            int pageSize);

        Task<IReadOnlyList<User>> GetUsersWithMessagesForAsync(long userId);
    }
}