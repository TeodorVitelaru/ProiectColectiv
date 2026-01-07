using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities
{
    /// <summary>
    /// Represents a match between two users.
    /// </summary>
    public class Match : Entity<long>
    {
        /// <summary>
        /// Gets or sets the ID of the user who initiated the match (liked).
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who initiated the match.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the matched user.
        /// </summary>
        public long MatchedUserId { get; set; }

        /// <summary>
        /// Gets or sets the matched user.
        /// </summary>
        public User MatchedUser { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether this is a mutual match (both liked each other).
        /// </summary>
        public bool IsMutual { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user liked (true) or disliked (false) the matched user.
        /// </summary>
        public bool IsLiked { get; set; } = true;

        /// <summary>
        /// Gets or sets the date when the match was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Factory method to create a new Match.
        /// </summary>
        public static Match Create(long userId, long matchedUserId, bool isMutual = false, bool isLiked = true)
        {
            return new Match
            {
                UserId = userId,
                MatchedUserId = matchedUserId,
                IsMutual = isMutual,
                IsLiked = isLiked,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Mark the match as mutual.
        /// </summary>
        public void MarkAsMutual()
        {
            IsMutual = true;
        }
    }
}

