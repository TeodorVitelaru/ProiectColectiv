namespace DatingApp.Dtos.Review;

public sealed class ReviewDto
{
    public long Id { get; set; }
    public long ReviewerId { get; set; }
    public long RevieweeId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}