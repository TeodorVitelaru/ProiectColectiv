namespace DatingApp.Dtos.Message
{
 public sealed class AddMessageRequest
 {
 public long UserId1 { get; set; }
 public long UserId2 { get; set; }
 public string Text { get; set; }
 }
}
