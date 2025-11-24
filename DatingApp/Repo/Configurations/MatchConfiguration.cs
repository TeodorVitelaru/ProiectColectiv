using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.UserId1)
            .IsRequired();

        builder.Property(m => m.UserId2)
            .IsRequired();

        builder.Property(m => m.MatchDate)
            .IsRequired();

    }
}
