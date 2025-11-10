using System.Data;
using DatingApp.Contracts.Persistence;
using DatingApp.Data;
using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repo;

internal sealed class ReportRepository : Repository<Report, long>, IReportRepository
{
    private readonly ProiectColectivContext _context;

    public ReportRepository(ProiectColectivContext context) : base(context)
    {
        _context = context;
    }

    public IDbConnection GetDbConnection() => _context.Database.GetDbConnection();
}


