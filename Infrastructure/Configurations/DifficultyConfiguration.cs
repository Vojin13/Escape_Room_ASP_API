using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Configurations
{
    public class DifficultyConfiguration : IEntityTypeConfiguration<Difficulty>
    {
        public void Configure(EntityTypeBuilder<Difficulty> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            var seededAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
            new Difficulty { Id = 1, Name = "Easy", CreatedAt = seededAt },
            new Difficulty { Id = 2, Name = "Medium", CreatedAt = seededAt },
            new Difficulty { Id = 3, Name = "Hard", CreatedAt = seededAt },
            new Difficulty { Id = 4, Name = "Expert", CreatedAt = seededAt }
        );
        }
    }
}
