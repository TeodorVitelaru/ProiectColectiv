namespace DatingApp.Dtos.Message
{
    public class GetMessagesBetween2UsersRequest
    {
        public long FirstUserId { get; set; }

        public long SecondUserId { get; set; }
    }
}
