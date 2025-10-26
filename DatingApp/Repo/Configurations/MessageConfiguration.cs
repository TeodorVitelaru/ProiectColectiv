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

 builder.Property(m => m.UserId1)
 .IsRequired();

 builder.Property(m => m.UserId2)
 .IsRequired();

 builder.Property(m => m.Text)
 .IsRequired()
 .HasMaxLength(1000);
 }
 }
}
