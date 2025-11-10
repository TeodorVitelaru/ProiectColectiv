using System.Data;
using DatingApp.Contracts.Persistence;
using DatingApp.Data;
using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repo;

internal sealed class ReviewRepository : Repository<Review, long>, IReviewRepository
{
    private readonly ProiectColectivContext _context;

    public ReviewRepository(ProiectColectivContext context) : base(context)
    {
        _context = context;
    }

    public IDbConnection GetDbConnection() => _context.Database.GetDbConnection();
}