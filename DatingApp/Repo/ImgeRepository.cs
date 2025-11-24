using DatingApp.Contracts.Persistence;
using DatingApp.Data;
using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.CompilerServices;


namespace DatingApp.Repo    
{
    internal sealed class ImageRepository : Repository<Image, long>, IImageRepository
    {  
        private readonly ProiectColectivContext _context;

        public ImageRepository(ProiectColectivContext context) : base(context)
        {
            _context = context;
        }

        public IDbConnection GetDbConnection() => _context.Database.GetDbConnection();

        public async Task<List<Image>> GetImagesByUserIdAsync(long userId)
        {
            return await _context.Set<Image>()
                .AsNoTracking()
                .Where(i => i.userId == userId)
                .ToListAsync();
        }
    }
}
