namespace DatingApp.Dtos.Message
{
 public sealed class MessageDto
 {
 public long Id { get; set; }
 public long SenderId { get; set; }
 public long RecipientId { get; set; }
 public string Text { get; set; }
 }
}
