using DatingApp.Contracts.Persistence;
using DatingApp.Data;
using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DatingApp.Repo
{
    internal sealed class UserRepository : Repository<User, long>, IUserRepository
    {
        private readonly ProiectColectivContext _context;

        public UserRepository(ProiectColectivContext context) : base(context)
        {
            _context = context;
        }

        public IDbConnection GetDbConnection() => _context.Database.GetDbConnection();
    }
}
