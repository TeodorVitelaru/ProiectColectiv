using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Repo.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.Property(r => r.ReviewerId).IsRequired();

        builder.Property(r => r.RevieweeId).IsRequired();

        builder.Property(r => r.Rating).IsRequired();
        builder.HasCheckConstraint("CK_Reviews_Rating_Range", "Rating BETWEEN 1 AND 5");

        builder.Property(r => r.Comment).IsRequired();
    }
}