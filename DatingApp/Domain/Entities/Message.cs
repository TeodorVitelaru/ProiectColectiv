using DatingApp.Domain.Primitives;

namespace DatingApp.Domain.Entities
{
 public class Message : Entity<long>
 {
 public long UserId1 { get; private set; }

 public long UserId2 { get; private set; }

 public string Text { get; private set; }

 protected Message(long id) : base(id) { }

 protected Message() : base() { }

 public static Message Create(long userId1, long userId2, string text)
 {
 if (string.IsNullOrWhiteSpace(text))
 throw new ArgumentException("Text cannot be empty.", nameof(text));

 return new Message()
 {
 UserId1 = userId1,
 UserId2 = userId2,
 Text = text
 };
 }

 public void UpdateText(string text)
 {
 if (string.IsNullOrWhiteSpace(text))
 throw new ArgumentException("Text cannot be empty.", nameof(text));

 Text = text;
 }
 }
}
