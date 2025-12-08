using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DatingApp.Domain.Entities;

namespace DatingApp.Repo.Configurations
{
    /// <summary>
    /// Entity Framework configuration for <see cref="UserLanguage"/>.
    /// </summary>
    public class UserLanguageConfiguration : IEntityTypeConfiguration<UserLanguage>
    {
        public void Configure(EntityTypeBuilder<UserLanguage> builder)
        {
            builder.ToTable("UserLanguages");

            builder.HasKey(ul => ul.Id);

            builder.Property(ul => ul.UserId)
                .IsRequired();

            builder.Property(ul => ul.Language)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(ul => ul.User)
                .WithMany(u => u.UserLanguages)
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ul => new { ul.UserId, ul.Language })
                .IsUnique();
        }
    }
}

