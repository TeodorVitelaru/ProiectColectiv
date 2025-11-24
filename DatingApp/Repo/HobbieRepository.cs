using DatingApp.Contracts.Persistence;
using DatingApp.Data;
using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DatingApp.Repo
{
    internal sealed class HobbieRepository : Repository<Hobbie, long>, IHobbieRepository
    {
        private readonly ProiectColectivContext _context;

        public HobbieRepository(ProiectColectivContext context) : base(context)
        {
            _context = context;
        }
        public IDbConnection GetDbConnection() => _context.Database.GetDbConnection();



    }
}
