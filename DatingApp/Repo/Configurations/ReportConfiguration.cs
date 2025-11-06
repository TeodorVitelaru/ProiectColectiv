using DatingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Repo.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("Reports");

        builder.Property(r => r.ReporterId).IsRequired();
        builder.Property(r => r.ReportedUserId).IsRequired();
        builder.Property(r => r.Reason).IsRequired();
    }
}


