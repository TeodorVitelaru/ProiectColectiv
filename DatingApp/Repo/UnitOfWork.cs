using DatingApp.Contracts.Persistence;
using DatingApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace DatingApp.Repo
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly ProiectColectivContext _context;
        private DbTransaction? _transaction;
        private DbConnection? _connection;

        public UnitOfWork(ILogger<UnitOfWork> logger, ProiectColectivContext context)
        {
            _logger = logger;
            _context = context;
            _connection = _context.Database.GetDbConnection();
            UserRepository = new UserRepository(context);
            MessageRepository = new MessageRepository(context);
        }

        public IUserRepository UserRepository { get; }

        public IMessageRepository MessageRepository { get; }

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        {
            if (_connection != null)
            {
                await _connection.OpenAsync();
                _transaction = await _connection.BeginTransactionAsync(isolationLevel);
                await _context.Database.UseTransactionAsync(_transaction);
            }
        }

        /// <inheritdoc/>
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        {
            _connection?.Open();
            _transaction = _connection?.BeginTransaction(isolationLevel);
            _context.Database.UseTransaction(_transaction);
        }

        /// <inheritdoc/>
        public Task<int> SaveChangesAsync()
        {
            try
            {
                return _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency violation error while saving to the database occurred");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error while saving to the database occurred");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error occurred");
                throw;
            }
        }

        /// <inheritdoc/>
        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
        }

        /// <inheritdoc/>
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
            }
        }

        /// <inheritdoc/>
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }

        /// <inheritdoc/>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;
            _connection?.Dispose();
            _connection = null;
            _context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
