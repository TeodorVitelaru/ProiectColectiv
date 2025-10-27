using DatingApp.Contracts.Persistence;
using DatingApp.Domain.Entities;
using DatingApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DatingApp.Repo
{
 internal sealed class MessageRepository : Repository<Message, long>, IMessageRepository
 {
 private readonly ProiectColectivContext _context;

 public MessageRepository(ProiectColectivContext context) : base(context)
 {
 _context = context;
 }

 public IDbConnection GetDbConnection() => _context.Database.GetDbConnection();
 }
}
