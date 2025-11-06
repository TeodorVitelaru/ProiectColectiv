using AutoMapper;
using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Contracts.Validators;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.Report;
using DatingApp.Exceptions;

namespace DatingApp.Service;

public class ReportService : IReportService
{
    private readonly ILogger<ReportService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRequestValidator _requestValidator;
    private readonly IMapper _mapper;

    public ReportService(ILogger<ReportService> logger, IUnitOfWork unitOfWork, IRequestValidator requestValidator,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _requestValidator = requestValidator;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReportDto>> GetAllReportsAsync()
    {
        _logger.LogTrace("Get All reports called");

        var reports = await _unitOfWork.ReportRepository.GetAllAsync();

        if (!reports.Any())
        {
            throw new NotFoundException("There are no reports");
        }

        return _mapper.Map<IEnumerable<ReportDto>>(reports);
    }

    public async Task<ReportDto> GetReportAsync(GetReportRequest request)
    {
        _logger.LogTrace("Get report called");

        _requestValidator.Validate(request);

        var report = await _unitOfWork.ReportRepository.FindFirstOrDefaultAsync(r => r.Id == request.Id) ??
                     throw new NotFoundException(nameof(Report), request.Id);
        return _mapper.Map<ReportDto>(report);
    }

    public async Task<ReportDto> AddReportAsync(AddReportRequest request)
    {
        _logger.LogTrace("Add report called");

        _requestValidator.Validate(request);

        // validate users exist
        var reporter = await _unitOfWork.UserRepository.GetByIdAsync(request.ReporterId);
        if (reporter == null)
            throw new NotFoundException("Reporter", request.ReporterId);

        var reported = await _unitOfWork.UserRepository.GetByIdAsync(request.ReportedUserId);
        if (reported == null)
            throw new NotFoundException("ReportedUser", request.ReportedUserId);

        var report = Report.Create(request.ReporterId, request.ReportedUserId, request.Reason);

        var added = await _unitOfWork.ReportRepository.AddAsync(report);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ReportDto>(added);
    }

    public async Task<ReportDto> EditReportAsync(EditReportRequest request)
    {
        _logger.LogTrace("Edit report called");

        _requestValidator.Validate(request);

        var existing = await _unitOfWork.ReportRepository.FindFirstOrDefaultAsync(r => r.Id == request.Id) ??
                       throw new NotFoundException(nameof(Report), request.Id);

        existing.UpdateReason(request.Reason);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ReportDto>(existing);
    }

    public async Task DeleteReportAsync(DeleteReportRequest request)
    {
        _logger.LogTrace("Delete report called");

        _requestValidator.Validate(request);

        var existing = await _unitOfWork.ReportRepository.GetByIdAsync(request.Id) ??
                       throw new NotFoundException(nameof(Report), request.Id);

        _unitOfWork.ReportRepository.RemoveById(existing.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}


