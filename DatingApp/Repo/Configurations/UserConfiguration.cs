﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DatingApp.Domain.Entities;

namespace DatingApp.Repo.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.IsAdmin)
                .IsRequired();

            // Registration profile fields
            builder.Property(u => u.Age)
                .IsRequired(false);

            builder.Property(u => u.Height)
                .IsRequired(false);

            builder.Property(u => u.Gender)
                .IsRequired(false)
                .HasConversion<int>();

            builder.Property(u => u.City)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(u => u.Bio)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(u => u.RelationshipGoal)
                .IsRequired(false)
                .HasConversion<int>();

            builder.Property(u => u.SexualOrientation)
                .IsRequired(false)
                .HasConversion<int>();

            builder.Property(u => u.PreferredAgeMin)
                .IsRequired(false);

            builder.Property(u => u.PreferredAgeMax)
                .IsRequired(false);

            // Relationships
            builder.HasMany(u => u.UserLanguages)
                .WithOne(ul => ul.User)
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserInterests)
                .WithOne(ui => ui.User)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Images)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
