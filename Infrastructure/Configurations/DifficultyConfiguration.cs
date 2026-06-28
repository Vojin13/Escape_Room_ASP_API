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

            builder.HasData(
            new Difficulty { Id = 1, Name = "Easy", CreatedAt = DateTime.UtcNow },
            new Difficulty { Id = 2, Name = "Medium", CreatedAt = DateTime.UtcNow },
            new Difficulty { Id = 3, Name = "Hard", CreatedAt = DateTime.UtcNow },
            new Difficulty { Id = 4, Name = "Expert", CreatedAt = DateTime.UtcNow }
        );
        }
    }
}
