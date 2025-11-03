namespace DatingApp.Dtos.Review;

public class AddReviewRequest
{
    public long ReviewerId { get; set; }
    public long RevieweeId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}