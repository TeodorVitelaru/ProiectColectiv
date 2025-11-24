using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DatingApp.Domain.Entities;

namespace DatingApp.Repo.Configurations
{
    public class HobbieConfiguration : IEntityTypeConfiguration<Hobbie>
    {
        public void Configure(EntityTypeBuilder<Hobbie> builder)
        {
            builder.ToTable("Hobbies");

            builder.Property(u => u.HobbieName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
