using DatingApp.Domain.Entities;

namespace DatingApp.Contracts.Persistence
{
    public interface IUserRepository : IRepository<User, long>
    {
        Task<User?> GetRandomUserAsync(int currentUserId);
        Task<User?> GetUserWithAllHobbies(long userId);
    }
}
