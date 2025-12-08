using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities
{
    /// <summary>
    /// Represents a language spoken by a user.
    /// </summary>
    public class UserLanguage : Entity<long>
    {
        public long UserId { get; private set; }
        public User User { get; private set; } = null!;
        public Enums.Language Language { get; private set; }

        protected UserLanguage(long id) : base(id) { }
        protected UserLanguage() : base() { }

        public static UserLanguage Create(long userId, Enums.Language language)
        {
            return new UserLanguage
            {
                UserId = userId,
                Language = language
            };
        }
    }
}

