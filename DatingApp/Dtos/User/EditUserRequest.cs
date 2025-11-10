namespace DatingApp.Dtos.User
{
    /// <summary>
    /// Request used for editing user info.
    /// </summary>
    public class EditUserRequest
    {
        /// <summary>
        /// Gets or sets user identifier.
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
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; } = null!;
    }
}
