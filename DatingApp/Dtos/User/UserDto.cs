using DatingApp.Enums;

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

        // Registration profile fields
        /// <summary>
        /// User age.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// User height in centimeters.
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// User gender.
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// User city.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// User bio/about section.
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// User relationship goal.
        /// </summary>
        public RelationshipGoal? RelationshipGoal { get; set; }

        /// <summary>
        /// User sexual orientation.
        /// </summary>
        public SexualOrientation? SexualOrientation { get; set; }

        /// <summary>
        /// Minimum preferred age.
        /// </summary>
        public int? PreferredAgeMin { get; set; }

        /// <summary>
        /// Maximum preferred age.
        /// </summary>
        public int? PreferredAgeMax { get; set; }

        /// <summary>
        /// User languages.
        /// </summary>
        public List<Language> Languages { get; set; } = new();

        /// <summary>
        /// User interests/passions.
        /// </summary>
        public List<Interest> Interests { get; set; } = new();

        /// <summary>
        /// User photos (as base64 strings).
        /// </summary>
        public List<string> Photos { get; set; } = new();
    }
}
