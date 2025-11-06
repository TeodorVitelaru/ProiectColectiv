using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities;

public class Report : Entity<long>
{
    public long ReporterId { get; private set; }

    public long ReportedUserId { get; private set; }

    public string Reason { get; private set; }

    protected Report(long id) : base(id)
    {
    }

    protected Report() : base()
    {
    }

    public static Report Create(long reporterId, long reportedUserId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason cannot be empty.", nameof(reason));

        return new Report()
        {
            ReporterId = reporterId,
            ReportedUserId = reportedUserId,
            Reason = reason
        };
    }

    public void UpdateReason(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason cannot be empty.", nameof(reason));

        Reason = reason;
    }
}


