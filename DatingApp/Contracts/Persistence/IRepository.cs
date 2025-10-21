using DatingApp.Domain.Primitives;
using System.Data;

namespace DatingApp.Contracts.Persistence
{
    public interface IRepository<TEntity, TKey> : IReadRepository<TEntity, TKey>, IWriteRepository<TEntity, TKey> where TEntity : Entity<TKey> where TKey : struct
    {
        /// <summary>
        /// Gets the current database connection from the DbContext instance
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDbConnection();

        /// <summary>
        /// Gets the current context transaction DbContext instance
        /// </summary>
        /// <returns></returns>
        public IDbTransaction? GetDbTransaction();
    }
}
