namespace DatingApp.Dtos.Report;

public class AddReportRequest
{
    public long ReporterId { get; set; }
    public long ReportedUserId { get; set; }
    public string Reason { get; set; }
}


