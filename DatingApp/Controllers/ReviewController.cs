using AutoMapper;
using DatingApp.Contracts.Services;
using DatingApp.Dtos.Review;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers;

[ApiController]
[Route("api/reviews")]
[Produces("application/json")]
public class ReviewController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IReviewService _reviewService;
    private readonly IMapper _mapper;

    public ReviewController(ILogger logger, IReviewService reviewService, IMapper mapper)
    {
        _logger = logger;
        _reviewService = reviewService;
        _mapper = mapper;

        _logger.LogTrace("{Controller} created", nameof(ReviewController));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll()
    {
        _logger.LogTrace("Get all reviews called");

        var reviews = await _reviewService.GetAllReviewsAsync();

        return reviews.ToList();
    }

    [HttpGet("{reviewId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewDto>> GetReviewAsync(long reviewId)
    {
        _logger.LogTrace($"Get review with ID {reviewId} called");

        ReviewDto reviewDto = await _reviewService.GetReviewAsync(new GetReviewRequest { Id = reviewId });

        return reviewDto;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReviewDto>> CreateReviewAsync(AddReviewRequest request)
    {
        _logger.LogTrace("Create review called");

        var response = await _reviewService.AddReviewAsync(request);

        return response;
    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReviewDto>> UpdateReviewAsync(long reviewId, EditReviewRequest request)
    {
        _logger.LogTrace($"Update review with ID {reviewId} called");

        if (reviewId != request.Id)
        {
            return BadRequest("Id doesn't match!");
        }

        var response = await _reviewService.EditReviewAsync(request);

        return response;
    }

    [HttpDelete("{reviewId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteUserAsync(long reviewId)
    {
        _logger.LogTrace($"Delete review with id {reviewId} called");

        await _reviewService.DeleteReviewAsync(new DeleteReviewRequest { Id = reviewId });

        return NoContent();
    }
}