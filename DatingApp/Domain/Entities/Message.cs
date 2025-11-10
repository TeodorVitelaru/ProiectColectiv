using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities
{
 public class Message : Entity<long>
 {
 public long SenderId { get; private set; }

 public User Sender { get; private set; }

 public long RecipientId { get; private set; }

 public User Recipient { get; private set; }

 public string Text { get; private set; }

 protected Message(long id) : base(id) { }

 protected Message() : base() { }

 public static Message Create(long senderId, long recipientId, string text)
 {
 if (string.IsNullOrWhiteSpace(text))
 throw new ArgumentException("Text cannot be empty.", nameof(text));

 return new Message()
 {
 SenderId = senderId,
 RecipientId = recipientId,
 Text = text
 };
 }

 public void UpdateText(string text)
 {
 if (string.IsNullOrWhiteSpace(text))
 throw new ArgumentException("Text cannot be empty.", nameof(text));

 Text = text;
 }

 public bool IsSender(long userId) => SenderId == userId;
 }
}
