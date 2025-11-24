namespace DatingApp.Dtos.Image
{
    public sealed class AddImageRequest
    {
        public long ImageId { get; set; }
        public string Image {  get; set; }

        public long userId {  get; set; }

    }
}
