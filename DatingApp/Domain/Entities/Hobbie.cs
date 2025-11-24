using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities
{
    public class Hobbie : Entity<long>
    {
        public string HobbieName { get; set; }

        protected Hobbie(long id) : base(id) { }

        protected Hobbie() : base() { }

        public ICollection<User> Users { get; private set; } = new List<User>();

        public static Hobbie Create(string hobbieName)
        {
            Hobbie hobbie = new()
            {
                HobbieName = hobbieName
            };

            return hobbie;
        }

        public void UpdateHobbieName(string hobbieName)
        {
            HobbieName = HobbieName;
        }
    }
}
