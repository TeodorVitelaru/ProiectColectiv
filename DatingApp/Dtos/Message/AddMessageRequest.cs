namespace DatingApp.Dtos.Message
{
 public sealed class AddMessageRequest
 {
 public long SenderId { get; set; }
 public long RecipientId { get; set; }
 public string Text { get; set; }
 }
}
