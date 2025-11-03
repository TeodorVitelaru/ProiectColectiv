using DatingApp.Dtos.Report;

namespace DatingApp.Contracts.Services;

public interface IReportService
{
    Task<IEnumerable<ReportDto>> GetAllReportsAsync();

    Task<ReportDto> GetReportAsync(GetReportRequest id);

    Task<ReportDto> AddReportAsync(AddReportRequest request);

    Task<ReportDto> EditReportAsync(EditReportRequest request);

    Task DeleteReportAsync(DeleteReportRequest request);
}


