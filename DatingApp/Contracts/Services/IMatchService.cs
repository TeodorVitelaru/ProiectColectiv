using DatingApp.Dtos.Match;

namespace DatingApp.Contracts.Services
{
    /// <summary>
    /// Contract for Match service operations.
    /// </summary>
    public interface IMatchService
    {
        /// <summary>
        /// Creates a new match (like) between two users.
        /// </summary>
        /// <param name="userId">The ID of the user creating the match.</param>
        /// <param name="request">The match request containing the matched user ID.</param>
        /// <returns>The created match response.</returns>
        Task<MatchResponse> AddMatchAsync(long userId, AddMatchRequest request);

        /// <summary>
        /// Gets all matches for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Collection of match DTOs.</returns>
        Task<IEnumerable<MatchDto>> GetUserMatchesAsync(long userId);

        /// <summary>
        /// Gets only mutual matches for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Collection of mutual match DTOs.</returns>
        Task<IEnumerable<MatchDto>> GetMutualMatchesAsync(long userId);

        /// <summary>
        /// Checks if two users have matched.
        /// </summary>
        /// <param name="userId1">First user ID.</param>
        /// <param name="userId2">Second user ID.</param>
        /// <returns>True if users have matched, false otherwise.</returns>
        Task<bool> AreMatchedAsync(long userId1, long userId2);

        /// <summary>
        /// Checks if users have a mutual match.
        /// </summary>
        /// <param name="userId1">First user ID.</param>
        /// <param name="userId2">Second user ID.</param>
        /// <returns>True if mutual, false otherwise.</returns>
        Task<bool> IsMutualMatchAsync(long userId1, long userId2);

        /// <summary>
        /// Deletes a match (unlike).
        /// </summary>
        /// <param name="matchId">The match ID to delete.</param>
        /// <returns>True if deleted successfully, false otherwise.</returns>
        Task<bool> DeleteMatchAsync(long matchId);

        /// <summary>
        /// Adds a dislike record for a user.
        /// </summary>
        /// <param name="userId">The ID of the user creating the dislike.</param>
        /// <param name="dislikedUserId">The ID of the user being disliked.</param>
        /// <returns>Response indicating the dislike was recorded.</returns>
        Task<MatchResponse> AddDislikeAsync(long userId, long dislikedUserId);

        /// <summary>
        /// Gets random unmatched users for a specific user.
        /// </summary>
        /// <param name="userId">The current user ID.</param>
        /// <param name="count">Number of random users to return.</param>
        /// <returns>Collection of unmatched users.</returns>
        Task<IEnumerable<Domain.Entities.User>> GetRandomUnmatchedUsersAsync(long userId, int count = 1);
    }
}
