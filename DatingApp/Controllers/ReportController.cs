using AutoMapper;
using DatingApp.Contracts.Services;
using DatingApp.Dtos.Report;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers;

[ApiController]
[Route("api/reports")]
[Produces("application/json")]
public class ReportController : ControllerBase
{
    private readonly ILogger<ReportController> _logger;
    private readonly IReportService _reportService;
    private readonly IMapper _mapper;

    public ReportController(ILogger<ReportController> logger, IReportService reportService, IMapper mapper)
    {
        _logger = logger;
        _reportService = reportService;
        _mapper = mapper;

        _logger.LogTrace("{Controller} created", nameof(ReportController));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ReportDto>>> GetAll()
    {
        _logger.LogTrace("Get all reports called");

        var reports = await _reportService.GetAllReportsAsync();

        return reports.ToList();
    }

    [HttpGet("{reportId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportDto>> GetReportAsync(long reportId)
    {
        _logger.LogTrace($"Get report with ID {reportId} called");

        ReportDto dto = await _reportService.GetReportAsync(new GetReportRequest { Id = reportId });

        return dto;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReportDto>> CreateReportAsync(AddReportRequest request)
    {
        _logger.LogTrace("Create report called");

        var response = await _reportService.AddReportAsync(request);

        return response;
    }

    [HttpPut("{reportId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReportDto>> UpdateReportAsync(long reportId, EditReportRequest request)
    {
        _logger.LogTrace($"Update report with ID {reportId} called");

        if (reportId != request.Id)
        {
            return BadRequest("Id doesn't match!");
        }

        var response = await _reportService.EditReportAsync(request);

        return response;
    }

    [HttpDelete("{reportId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteReportAsync(long reportId)
    {
        _logger.LogTrace($"Delete report with id {reportId} called");

        await _reportService.DeleteReportAsync(new DeleteReportRequest { Id = reportId });

        return NoContent();
    }
}


