using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities
{
    public class Hobbie : Entity<long>
    {
        public string HobbieName { get; set; }

        protected Hobbie(long id) : base(id) { }

        protected Hobbie() : base() { }

        public static Hobbie Create(string hobbieName)
        {
            Hobbie hobbie = new()
            {
                HobbieName = hobbieName
            };

            return hobbie;
        }
    }
}
