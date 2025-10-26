using DatingApp.Domain.Primitives;
using System.Linq.Expressions;

namespace DatingApp.Contracts.Persistence
{
    public interface IReadRepository<TEntity, TKey> where TEntity : Entity<TKey> where TKey : struct
    {
        /// <summary>
        /// Asynchronously gets the <typeparamref name="TEntity"/> by identifier.
        /// </summary>
        /// <param name="id"><typeparamref name="TKey"/> identifier.</param>
        /// <returns><typeparamref name="TEntity"/> if entity exists; <see langword="null"/> otherwise.</returns>
        Task<TEntity?> GetByIdAsync(TKey id);

        /// <summary>
        /// Asynchronously find all entities <typeparamref name="TEntity"/> by identifier.
        /// </summary>
        /// <param name="ids"><typeparamref name="TKey"/> identifier.</param>
        /// <returns><typeparamref name="TEntity"/> if entities exist; <see langword="null"/> otherwise.</returns>
        Task<ICollection<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids);

        /// <summary>
        /// Asynchronously gets the collection of all <typeparamref name="TEntity"/> entities.
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Asynchronously finds entities using a <paramref name="predicate"/> filter condition.
        /// </summary>
        /// <param name="predicate">Filter condition.</param>
        /// <param name="paths">Including entities.</param>
        /// <returns>Collection of <typeparamref name="TEntity"/> entities.</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] paths);

        /// <summary>
        /// Asynchronously finds first or default entity using <paramref name="predicate"/> filter condition.
        /// </summary>
        /// <param name="predicate">Filter condition.</param>
        /// <param name="paths">Including entities.</param>
        /// <returns>First or default <typeparamref name="TEntity"/> entity.</returns>
        Task<TEntity?> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] paths);
    }
}
