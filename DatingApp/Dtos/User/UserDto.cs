namespace DatingApp.Dtos.User
{
    /// <summary>
    /// DTO used for user data.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// User first name.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// User last name.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// User password.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// User is admin.
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
