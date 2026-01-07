using DatingApp.Contracts.Persistence;
using DatingApp.Domain.Entities;
using DatingApp.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repo
{
    /// <summary>
    /// Repository for Match entity operations.
    /// </summary>
    internal sealed class MatchRepository : Repository<Match, long>, IMatchRepository
    {
        private readonly ProiectColectivContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public MatchRepository(ProiectColectivContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all matches for a specific user (both sent and mutual matches).
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Collection of matches.</returns>
        public async Task<IEnumerable<Match>> GetUserMatchesAsync(long userId)
        {
            return await _context.Matches
                .Where(m => (m.UserId == userId || m.MatchedUserId == userId) && m.IsLiked)
                .Include(m => m.User)
                    .ThenInclude(u => u.Images)
                .Include(m => m.MatchedUser)
                    .ThenInclude(mu => mu.Images)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets only mutual matches for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Collection of mutual matches.</returns>
        public async Task<IEnumerable<Match>> GetMutualMatchesAsync(long userId)
        {
            return await _context.Matches
                .Where(m => m.UserId == userId && m.IsMutual)
                .Include(m => m.User)
                .Include(m => m.MatchedUser)
                    .ThenInclude(mu => mu.Images)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a match exists between two users.
        /// </summary>
        /// <param name="userId1">First user ID.</param>
        /// <param name="userId2">Second user ID.</param>
        /// <returns>The match if found, otherwise null.</returns>
        public async Task<Match?> FindByUserIdsAsync(long userId1, long userId2)
        {
            return await _context.Matches
                .FirstOrDefaultAsync(m =>
                    (m.UserId == userId1 && m.MatchedUserId == userId2) ||
                    (m.UserId == userId2 && m.MatchedUserId == userId1));
        }

        /// <summary>
        /// Gets a match by ID.
        /// </summary>
        /// <param name="id">The match ID.</param>
        /// <returns>The match if found, otherwise null.</returns>
        public async Task<Match?> GetByIdAsync(long id)
        {
            return await _context.Matches
                .Include(m => m.User)
                .Include(m => m.MatchedUser)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// Adds a new match.
        /// </summary>
        /// <param name="match">The match to add.</param>
        public void Add(Match match)
        {
            _context.Matches.Add(match);
        }

        /// <summary>
        /// Updates an existing match.
        /// </summary>
        /// <param name="match">The match to update.</param>
        public void Update(Match match)
        {
            _context.Matches.Update(match);
        }

        /// <summary>
        /// Removes a match.
        /// </summary>
        /// <param name="match">The match to remove.</param>
        public void Remove(Match match)
        {
            _context.Matches.Remove(match);
        }

        /// <summary>
        /// Gets unmatched random users for a specific user.
        /// Filters based on current user's sexual orientation and gender preferences.
        /// </summary>
        /// <param name="userId">The current user ID.</param>
        /// <param name="count">Number of random users to return.</param>
        /// <returns>Collection of random unmatched users.</returns>
        public async Task<IEnumerable<User>> GetRandomUnmatchedUsersAsync(long userId, int count = 1)
        {
            // Get current user with their preferences
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (currentUser == null)
            {
                return Enumerable.Empty<User>();
            }

            // Get all users that the current user has already liked or disliked
            var excludedUserIds = await _context.Matches
                .Where(m => m.UserId == userId)
                .Select(m => m.MatchedUserId)
                .ToListAsync();

            excludedUserIds.Add(userId); // Exclude self

            // Build query with gender filtering based on sexual orientation
            var query = _context.Users
                .Include(u => u.Images)
                .Where(u => !excludedUserIds.Contains(u.Id));

            // Apply gender filtering based on sexual orientation
            if (currentUser.SexualOrientation == Enums.SexualOrientation.Straight)
            {
                // Straight: show opposite gender only
                if (currentUser.Gender == Enums.Gender.Male)
                {
                    query = query.Where(u => u.Gender == Enums.Gender.Female);
                }
                else if (currentUser.Gender == Enums.Gender.Female)
                {
                    query = query.Where(u => u.Gender == Enums.Gender.Male);
                }
            }
            else if (currentUser.SexualOrientation == Enums.SexualOrientation.Gay)
            {
                // Gay men: show males only
                query = query.Where(u => u.Gender == Enums.Gender.Male);
            }
            else if (currentUser.SexualOrientation == Enums.SexualOrientation.Lesbian)
            {
                // Lesbian women: show females only
                query = query.Where(u => u.Gender == Enums.Gender.Female);
            }
            else if (currentUser.SexualOrientation == Enums.SexualOrientation.Bisexual)
            {
                // Bisexual: show both males and females
                query = query.Where(u => u.Gender == Enums.Gender.Male || u.Gender == Enums.Gender.Female);
            }
            // For "Other" orientation, show all users (no gender filter)

            return await query
                .OrderBy(u => EF.Functions.Random())
                .Take(count)
                .ToListAsync();
        }
    }
}
