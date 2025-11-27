namespace DatingApp.Dtos.Message
{
    /// <summary>
    /// Request model for retrieving the conversation partners for a user.
    /// </summary>
    public class GetUsersWithMessagesRequest
    {
        /// <summary>
        /// The user for whom to retrieve the conversation partners.
        /// </summary>
        public long UserId { get; set; }
    }
}

