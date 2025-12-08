using DatingApp.Domain.Entities;

namespace DatingApp.Contracts.Persistence
{
    /// <summary>
    /// Repository interface for <see cref="UserLanguage"/> entity.
    /// </summary>
    public interface IUserLanguageRepository : IRepository<UserLanguage, long>
    {
        /// <summary>
        /// Gets all languages for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Collection of user languages.</returns>
        Task<IEnumerable<UserLanguage>> GetByUserIdAsync(long userId);

        /// <summary>
        /// Deletes all languages for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        Task DeleteByUserIdAsync(long userId);
    }
}

