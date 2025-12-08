namespace DatingApp.Dtos.User
{
    /// <summary>
    /// Response returned after setting up user profile.
    /// </summary>
    public class SetupProfileResponse
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
        /// Indicates whether the profile was successfully completed.
        /// </summary>
        public bool ProfileCompleted { get; set; }
    }
}

