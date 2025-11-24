namespace DatingApp.Dtos.Image
{
    public sealed class ImageDto
    {
        public long Id { get; set; }

        public long userId { get; set; }

        public string ImageBase64 { get; set; }
    }
}
