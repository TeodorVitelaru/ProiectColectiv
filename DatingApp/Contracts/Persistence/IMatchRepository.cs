namespace DatingApp.Contracts.Persistence
{
    /// <summary>
    /// Contract for Match repository operations.
    /// </summary>
    public interface IMatchRepository
    {
        /// <summary>
        /// Gets all matches for a specific user (both sent and mutual matches).
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Collection of matches.</returns>
        Task<IEnumerable<Domain.Entities.Match>> GetUserMatchesAsync(long userId);

        /// <summary>
        /// Gets only mutual matches for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Collection of mutual matches.</returns>
        Task<IEnumerable<Domain.Entities.Match>> GetMutualMatchesAsync(long userId);

        /// <summary>
        /// Checks if a match exists between two users.
        /// </summary>
        /// <param name="userId1">First user ID.</param>
        /// <param name="userId2">Second user ID.</param>
        /// <returns>The match if found, otherwise null.</returns>
        Task<Domain.Entities.Match?> FindByUserIdsAsync(long userId1, long userId2);

        /// <summary>
        /// Gets a match by ID.
        /// </summary>
        /// <param name="id">The match ID.</param>
        /// <returns>The match if found, otherwise null.</returns>
        Task<Domain.Entities.Match?> GetByIdAsync(long id);

        /// <summary>
        /// Adds a new match.
        /// </summary>
        /// <param name="match">The match to add.</param>
        void Add(Domain.Entities.Match match);

        /// <summary>
        /// Updates an existing match.
        /// </summary>
        /// <param name="match">The match to update.</param>
        void Update(Domain.Entities.Match match);

        /// <summary>
        /// Removes a match.
        /// </summary>
        /// <param name="match">The match to remove.</param>
        void Remove(Domain.Entities.Match match);

        /// <summary>
        /// Gets unmatched random users for a specific user.
        /// </summary>
        /// <param name="userId">The current user ID.</param>
        /// <param name="count">Number of random users to return.</param>
        /// <returns>Collection of random unmatched users.</returns>
        Task<IEnumerable<Domain.Entities.User>> GetRandomUnmatchedUsersAsync(long userId, int count = 1);
    }
}
