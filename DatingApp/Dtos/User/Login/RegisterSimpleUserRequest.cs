namespace DatingApp.Dtos.User.Login
{
    /// <summary>
    /// Simple registration request with only basic credentials.
    /// Complete profile setup happens in setup-profile flow.
    /// </summary>
    public class RegisterSimpleUserRequest
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
    }
}
