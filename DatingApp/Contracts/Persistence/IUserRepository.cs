using DatingApp.Domain.Entities;

namespace DatingApp.Contracts.Persistence
{
    public interface IUserRepository : IRepository<User, long>
    {
    }
}
