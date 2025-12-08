using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities
{
    /// <summary>
    /// Represents an interest/passion of a user.
    /// </summary>
    public class UserInterest : Entity<long>
    {
        public long UserId { get; private set; }
        public User User { get; private set; } = null!;
        public Enums.Interest Interest { get; private set; }

        protected UserInterest(long id) : base(id) { }
        protected UserInterest() : base() { }

        public static UserInterest Create(long userId, Enums.Interest interest)
        {
            return new UserInterest
            {
                UserId = userId,
                Interest = interest
            };
        }
    }
}

