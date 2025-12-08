using DatingApp.Domain.Entities;

namespace DatingApp.Contracts.Persistence
{
    /// <summary>
    /// Repository interface for <see cref="UserInterest"/> entity.
    /// </summary>
    public interface IUserInterestRepository : IRepository<UserInterest, long>
    {
        /// <summary>
        /// Gets all interests for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Collection of user interests.</returns>
        Task<IEnumerable<UserInterest>> GetByUserIdAsync(long userId);

        /// <summary>
        /// Deletes all interests for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        Task DeleteByUserIdAsync(long userId);
    }
}

