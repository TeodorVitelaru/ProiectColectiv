using DatingApp.Domain.Primitives;
namespace DatingApp.Domain.Entities
{
    public class Match : Entity<long>
    {
       

        public long UserId1 {  get; set; }
        public long UserId2 { get; set; }
        public DateTime MatchDate {  get; set; }

        protected Match(long id) : base(id) { }

        protected Match() : base() { }

        public Match(long user1, long user2, DateTime dt)
        {
            UserId1 = user1;
            UserId2 = user2;
            MatchDate = dt;
        }

        public static Match Create(long userId1, long userId2, DateTime matchDate)
        {
            return new Match(userId1, userId2, matchDate);
        }
    }
}
