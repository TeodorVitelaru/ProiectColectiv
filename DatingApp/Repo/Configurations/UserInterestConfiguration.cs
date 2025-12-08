using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DatingApp.Domain.Entities;

namespace DatingApp.Repo.Configurations
{
    /// <summary>
    /// Entity Framework configuration for <see cref="UserInterest"/>.
    /// </summary>
    public class UserInterestConfiguration : IEntityTypeConfiguration<UserInterest>
    {
        public void Configure(EntityTypeBuilder<UserInterest> builder)
        {
            builder.ToTable("UserInterests");

            builder.HasKey(ui => ui.Id);

            builder.Property(ui => ui.UserId)
                .IsRequired();

            builder.Property(ui => ui.Interest)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(ui => ui.User)
                .WithMany(u => u.UserInterests)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ui => new { ui.UserId, ui.Interest })
                .IsUnique();
        }
    }
}

