using DatingApp.Dtos.Review;

namespace DatingApp.Contracts.Services;

public interface IReviewService
{
    Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();

    Task<ReviewDto> GetReviewAsync(GetReviewRequest id);

    Task<ReviewDto> AddReviewAsync(AddReviewRequest request);

    Task<ReviewDto> EditReviewAsync(EditReviewRequest request);

    Task DeleteReviewAsync(DeleteReviewRequest request);
}