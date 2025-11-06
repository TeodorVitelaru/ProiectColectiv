namespace DatingApp.Dtos.Report;

public sealed class ReportDto
{
    public long Id { get; set; }
    public long ReporterId { get; set; }
    public long ReportedUserId { get; set; }
    public string Reason { get; set; }
}


