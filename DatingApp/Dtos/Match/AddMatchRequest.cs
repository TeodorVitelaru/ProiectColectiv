namespace DatingApp.Dtos.Match
{
    /// <summary>
    /// Request DTO for adding a new match (liking a user).
    /// </summary>
    public class AddMatchRequest
    {
        /// <summary>
        /// Gets or sets the ID of the user to match with.
        /// </summary>
        public long MatchedUserId { get; set; }
    }

    /// <summary>
    /// Response DTO for match operations.
    /// </summary>
    public class MatchResponse
    {
        /// <summary>
        /// Gets or sets the match ID.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a mutual match.
        /// </summary>
        public bool IsMutual { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; } = null!;
    }
}
