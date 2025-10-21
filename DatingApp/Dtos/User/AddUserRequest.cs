namespace DatingApp.Dtos.User
{
    /// <summary>
    /// Request used for adding user.
    /// </summary>
    public class AddUserRequest
    {
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

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Gets or sets isAdmin.
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
