using AutoMapper;
using DatingApp.Contracts.Persistence;
using DatingApp.Contracts.Services;
using DatingApp.Contracts.Validators;
using DatingApp.Domain.Entities;
using DatingApp.Dtos.Review;
using DatingApp.Exceptions;

namespace DatingApp.Service;

public class ReviewService : IReviewService
{
    private readonly ILogger<ReviewService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRequestValidator _requestValidator;
    private readonly IMapper _mapper;

    public ReviewService(ILogger<ReviewService> logger, IUnitOfWork unitOfWork, IRequestValidator requestValidator,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _requestValidator = requestValidator;
        _mapper = mapper;
    }


    public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
    {
        _logger.LogTrace("Get All reviews called");

        var reviews = await _unitOfWork.ReviewRepository.GetAllAsync();

        if (!reviews.Any())
        {
            throw new NotFoundException("There are no reviews");
        }

        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<ReviewDto> GetReviewAsync(GetReviewRequest request)
    {
        _logger.LogTrace("Get review called");

        _requestValidator.Validate(request);

        var review = await _unitOfWork.ReviewRepository.FindFirstOrDefaultAsync(r => r.Id == request.Id) ??
                     throw new NotFoundException(nameof(Review), request.Id);
        return _mapper.Map<ReviewDto>(review);
    }

    public async Task<ReviewDto> AddReviewAsync(AddReviewRequest request)
    {
        _logger.LogTrace("Add review called");

        _requestValidator.Validate(request);

        // validate users exist
        var reviewer = await _unitOfWork.UserRepository.GetByIdAsync(request.ReviewerId);
        if (reviewer == null)
            throw new NotFoundException("Reviewer", request.ReviewerId);

        var reviewee = await _unitOfWork.UserRepository.GetByIdAsync(request.RevieweeId);
        if (reviewee == null)
            throw new NotFoundException("Reviewee", request.RevieweeId);

        var review = Review.Create(request.ReviewerId, request.RevieweeId, request.Rating, request.Comment);

        var addedReview = await _unitOfWork.ReviewRepository.AddAsync(review);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ReviewDto>(addedReview);
    }

    public async Task<ReviewDto> EditReviewAsync(EditReviewRequest request)
    {
        _logger.LogTrace("Edit review called");

        _requestValidator.Validate(request);

        var existing = await _unitOfWork.ReviewRepository.FindFirstOrDefaultAsync(r => r.Id == request.Id) ??
                       throw new NotFoundException(nameof(Review), request.Id);

        existing.UpdateRating(request.Rating);
        existing.UpdateComment(request.Comment);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ReviewDto>(existing);
    }

    public async Task DeleteReviewAsync(DeleteReviewRequest request)
    {
        _logger.LogTrace("Delete review called");

        _requestValidator.Validate(request);

        var existing = await _unitOfWork.ReviewRepository.GetByIdAsync(request.Id) ??
                       throw new NotFoundException(nameof(Review), request.Id);

        _unitOfWork.ReviewRepository.RemoveById(existing.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}