using DatingApp.Domain.Entities;

namespace DatingApp.Contracts.Persistence
{
 public interface IMessageRepository : IRepository<Message, long>
 {
 }
}
