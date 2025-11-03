using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities;

public class Review : Entity<long>
{
    public long ReviewerId { get; private set; }

    public long RevieweeId { get; private set; }

    public int Rating { get; private set; }

    public string Comment { get; private set; }

    protected Review(long id) : base(id)
    {
    }

    protected Review() : base()
    {
    }

    public static Review Create(long reviewerId, long revieweeId, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));

        return new Review()
        {
            ReviewerId = reviewerId,
            RevieweeId = revieweeId,
            Rating = rating,
            Comment = comment
        };
    }

    public void UpdateRating(int rating)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));

        Rating = rating;
    }

    public void UpdateComment(string comment)
    {
        Comment = comment;
    }
}