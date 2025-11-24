using DatingApp.Domain.Primitives;
using static System.Net.Mime.MediaTypeNames;
namespace DatingApp.Domain.Entities
{
    public class Image : Entity<long>
    {
        public byte[] image { get; set; }
        public long userId {  get; set; }

        public static Image Create(byte[] image,long userId)
        {
           return new Image() { image = image, userId = userId };
        }


    }

}
