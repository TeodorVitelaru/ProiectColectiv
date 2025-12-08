using DatingApp.Contracts.Persistence;
using DatingApp.Data;
using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repo
{
    /// <summary>
    /// Repository implementation for <see cref="UserLanguage"/> entity.
    /// </summary>
    internal class UserLanguageRepository : Repository<UserLanguage, long>, IUserLanguageRepository
    {
        private readonly ProiectColectivContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLanguageRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserLanguageRepository(ProiectColectivContext context) : base(context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserLanguage>> GetByUserIdAsync(long userId)
        {
            return await _context.Set<UserLanguage>()
                .Where(ul => ul.UserId == userId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteByUserIdAsync(long userId)
        {
            var userLanguages = await GetByUserIdAsync(userId);
            _context.Set<UserLanguage>().RemoveRange(userLanguages);
        }
    }
}

