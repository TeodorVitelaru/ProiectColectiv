using DatingApp.Contracts.Persistence;
using DatingApp.Domain.Entities;
using DatingApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DatingApp.Repo
{
    internal sealed class MatchRepository : Repository<Match, long>, IMatchRepository
    {
        private readonly ProiectColectivContext _context;

        public MatchRepository(ProiectColectivContext context) : base(context)
        {
            _context = context;
        }

        public IDbConnection GetDbConnection() => _context.Database.GetDbConnection();
    }
}
