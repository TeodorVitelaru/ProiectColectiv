using DatingApp.Contracts.Persistence;
using DatingApp.Data;
using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DatingApp.Repo
{
    internal sealed class UserRepository : Repository<User, long>, IUserRepository
    {
        private readonly ProiectColectivContext _context;

        public UserRepository(ProiectColectivContext context) : base(context)
        {
            _context = context;
        }

        public IDbConnection GetDbConnection() => _context.Database.GetDbConnection();

        public async Task<User?> GetRandomUserAsync(int currentUserId)
        {
            var sentMessagesToUsers = await _context.Messages
                .Where(m => m.SenderId == currentUserId)
                .Select(m => m.RecipientId)
                .ToListAsync();

            var usersToExclude = sentMessagesToUsers.Append(currentUserId);

            var eligibleUsersCount = await _context.Users
                .Where(u => !usersToExclude.Contains(u.Id))
                .CountAsync();

            if (eligibleUsersCount == 0)
            {
                return null;
            }

            var randomIndex = new Random().Next(0, eligibleUsersCount);

            var randomUser = await _context.Users
                .Where(u => !usersToExclude.Contains(u.Id))
                .OrderBy(u => u.Id)
                .Skip(randomIndex)
                .FirstOrDefaultAsync();

            return randomUser;
        }
    }
}