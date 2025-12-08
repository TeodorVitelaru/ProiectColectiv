using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities
{
    public class Image : Entity<long>
    {
        public byte[] ImageData { get; private set; } = null!;
        public long UserId { get; private set; }
        public User User { get; private set; } = null!;


        protected Image(long id) : base(id) { }
        protected Image() { }

        public static Image Create(byte[] imageData, long userId)
        {
            return new Image() 
            { 
                ImageData = imageData, 
                UserId = userId 
            };
        }
    }

}
