using DatingApp.Contracts.Persistence;
using DatingApp.Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DatingApp.Repo
{
    internal abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey> where TKey : struct
    {
        private readonly DbSet<TEntity> _entities;
        private readonly DbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity, TKey}"/> class.
        /// </summary>
        public Repository(DbContext context)
        {
            _entities = context.Set<TEntity>();
            _context = context;
        }

        /// <inheritdoc/>
        public IDbConnection GetDbConnection() => _context.Database.GetDbConnection();

        /// <inheritdoc/>
        public IDbTransaction? GetDbTransaction() => _context.Database.CurrentTransaction?.GetDbTransaction();

        /// <inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _entities.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task<ICollection<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids)
        {
            var entity = await _entities
                .Where(_entities => ids.Contains(_entities.Id))
                .ToListAsync();

            return entity;
        }

        /// <inheritdoc/>a
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] paths)
        {
            var query = _entities.Where(predicate);

            foreach (var path in paths)
            {
                query = query.Include(path);
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TEntity?> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] paths)
        {
            var query = _entities.Where(predicate);

            foreach (var path in paths)
            {
                query = query.Include(path);
            }

            return await query.FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] paths)
        {
            var query = _entities.Where(predicate);

            foreach (var path in paths)
            {
                query = query.Include(path);
            }

            return query;
        }

        /// <inheritdoc/>
        public TEntity Add(TEntity entity)
        {
            return _entities.Add(entity).Entity;
        }

        /// <inheritdoc/>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            return (await _entities.AddAsync(entity)).Entity;
        }

        /// <inheritdoc/>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        /// <inheritdoc/>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        /// <inheritdoc/>
        public void RemoveById(TKey id)
        {
            TEntity? entity = _entities.Find(id);

            if (entity is not null)
            {
                _entities.Remove(entity);
            }
        }

        /// <inheritdoc/>
        public async Task RemoveByIdAsync(TKey id)
        {
            TEntity? entity = await _entities.FindAsync(id);

            if (entity is not null)
            {
                _entities.Remove(entity);
            }
        }

        /// <inheritdoc/>
        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        /// <inheritdoc/>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }
    }
}
