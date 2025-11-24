using DatingApp.Contracts.Persistence;
using DatingApp.Domain.Entities;
using DatingApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DatingApp.Repo
{
    internal sealed class MessageRepository : Repository<Message, long>, IMessageRepository
    {
        private readonly ProiectColectivContext _context;

        public MessageRepository(ProiectColectivContext context) : base(context)
        {
            _context = context;
        }

        public IDbConnection GetDbConnection() => _context.Database.GetDbConnection();

        public async Task<(List<Message> messages, int totalCount)> GetPaginatedMessagesBetweenTwoUsersAsync(
            long senderId,
            long recipientId,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Messages
                .Where(m =>
                    (m.SenderId == senderId && m.RecipientId == recipientId)
                    ||
                    (m.SenderId == recipientId && m.RecipientId == senderId))
                .OrderBy(m => m.SentAt);

            var totalCount = await query.CountAsync();

            var messages = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (messages, totalCount);
        }
    }
}