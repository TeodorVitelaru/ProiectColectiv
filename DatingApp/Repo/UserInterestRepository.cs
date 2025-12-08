using DatingApp.Contracts.Persistence;
using DatingApp.Data;
using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repo
{
    /// <summary>
    /// Repository implementation for <see cref="UserInterest"/> entity.
    /// </summary>
    internal class UserInterestRepository : Repository<UserInterest, long>, IUserInterestRepository
    {
        private readonly ProiectColectivContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterestRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserInterestRepository(ProiectColectivContext context) : base(context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserInterest>> GetByUserIdAsync(long userId)
        {
            return await _context.Set<UserInterest>()
                .Where(ui => ui.UserId == userId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteByUserIdAsync(long userId)
        {
            var userInterests = await GetByUserIdAsync(userId);
            _context.Set<UserInterest>().RemoveRange(userInterests);
        }
    }
}

