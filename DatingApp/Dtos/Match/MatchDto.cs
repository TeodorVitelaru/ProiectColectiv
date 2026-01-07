namespace DatingApp.Dtos.Match
{
    /// <summary>
    /// DTO for Match entity.
    /// </summary>
    public class MatchDto
    {
        /// <summary>
        /// Gets or sets the match ID.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who liked.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the matched user.
        /// </summary>
        public long MatchedUserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a mutual match.
        /// </summary>
        public bool IsMutual { get; set; }

        /// <summary>
        /// Gets or sets the date when the match was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the matched user details.
        /// </summary>
        public MatchedUserDto? MatchedUserDetails { get; set; }
    }

    /// <summary>
    /// Detailed DTO for matched user in a match.
    /// </summary>
    public class MatchedUserDto
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user first name.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user last name.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user age.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the user location.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Gets or sets the user bio.
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// Gets or sets the user's primary photo URL.
        /// </summary>
        public string? ProfilePhotoUrl { get; set; }
    }
}
