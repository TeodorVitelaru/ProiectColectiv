using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Repo.Configurations
{
    /// <summary>
    /// Entity configuration for Match entity.
    /// </summary>
    public sealed class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        /// <summary>
        /// Configures the Match entity.
        /// </summary>
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.UserId)
                .IsRequired();

            builder.Property(m => m.MatchedUserId)
                .IsRequired();

            builder.Property(m => m.IsMutual)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(m => m.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Configure foreign keys with NO ACTION to avoid cascade cycles
            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(m => m.MatchedUser)
                .WithMany()
                .HasForeignKey(m => m.MatchedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Add unique constraint to prevent duplicate matches in one direction
            builder.HasIndex(m => new { m.UserId, m.MatchedUserId })
                .IsUnique()
                .HasName("IX_Match_UserIds_Unique");
        }
    }
}
