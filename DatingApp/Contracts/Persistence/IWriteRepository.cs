using DatingApp.Domain.Primitives;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DatingApp.Contracts.Persistence
{
    public interface IWriteRepository<TEntity, TKey> where TEntity : Entity<TKey> where TKey : struct
    {
        /// <summary>
        /// Adds <typeparamref name="TEntity"/> entity.
        /// </summary>
        /// <param name="entity"><typeparamref name="TEntity"/> to add.</param>
        /// <returns>Added <typeparamref name="TEntity"/> entity.</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Asynchronously adds <typeparamref name="TEntity"/> entity.
        /// </summary>
        /// <param name="entity"><typeparamref name="TEntity"/> to add.</param>
        /// <returns>Added <typeparamref name="TEntity"/> entity.</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Adds the collection of <typeparamref name="TEntity"/> entities.
        /// </summary>
        /// <param name="entities">Collection of <typeparamref name="TEntity"/> entities to add.</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously adds the collection of <typeparamref name="TEntity"/> entities.
        /// </summary>
        /// <param name="entities">Collection of <typeparamref name="TEntity"/> entities to add.</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes the <typeparamref name="TEntity"/> by identifier.
        /// </summary>
        /// <param name="id"><typeparamref name="TKey"/> entity identifier.</param>
        void RemoveById(TKey id);

        /// <summary>
        /// Asynchronously removes the <typeparamref name="TEntity"/> by identifier.
        /// </summary>
        /// <param name="id"><typeparamref name="TKey"/> entity identifier.</param>
        Task RemoveByIdAsync(TKey id);

        /// <summary>
        /// Removes the <typeparamref name="TEntity"/> entity.
        /// </summary>
        /// <param name="entity"><typeparamref name="TEntity"/> entity to remove.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Removes the collection of <typeparamref name="TEntity"/> entities.
        /// </summary>
        /// <param name="entities">Collection of <typeparamref name="TEntity"/> entities to remove.</param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
