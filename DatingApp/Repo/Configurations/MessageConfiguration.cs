using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DatingApp.Domain.Entities;

namespace DatingApp.Repo.Configurations
{
 public class MessageConfiguration : IEntityTypeConfiguration<Message>
 {
 public void Configure(EntityTypeBuilder<Message> builder)
 {
 builder.ToTable("Messages");

 builder.HasKey(m => m.Id);

 builder.Property(m => m.SenderId)
 .IsRequired()
 .HasColumnName("UserId1");

 builder.Property(m => m.RecipientId)
 .IsRequired()
 .HasColumnName("UserId2");

 builder.Property(m => m.Text)
 .IsRequired()
 .HasMaxLength(1000);

 // Relationship: Sender (User) -> SentMessages (if exists)
 builder.HasOne(m => m.Sender)
 .WithMany()
 .HasForeignKey(m => m.SenderId)
 .OnDelete(DeleteBehavior.Restrict);

 // Relationship: Recipient (User) -> ReceivedMessages (if exists)
 builder.HasOne(m => m.Recipient)
 .WithMany()
 .HasForeignKey(m => m.RecipientId)
 .OnDelete(DeleteBehavior.Restrict);
 }
 }
}
