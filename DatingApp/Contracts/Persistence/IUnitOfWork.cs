using System.Data;

namespace DatingApp.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        /// <summary>
        /// Saves the changes to database asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task{T}"/> that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Opens the current context database connection and begins the transaction asynchroniously
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted);

        /// <summary>
        /// Opens the current context database connection and begins the transaction
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted);

        /// <summary>
        /// Commits the current context's transaction and closes the connection asynchroniously
        /// </summary>
        /// <returns></returns>
        Task CommitTransactionAsync();

        /// <summary>
        /// Commits the current context's transaction and closes the connection
        /// </summary>
        /// <returns></returns>
        void CommitTransaction();

        /// <summary>
        /// Rollbacks of the database changes in the current context transaction scope and closes the connection asynchroniously
        /// </summary>
        /// <returns></returns>
        Task RollbackTransactionAsync();

        /// <summary>
        /// Rollbacks of the database changes in the current context transaction scope and closes the connection
        /// </summary>
        /// <returns></returns>
        void RollbackTransaction();
    }
}
