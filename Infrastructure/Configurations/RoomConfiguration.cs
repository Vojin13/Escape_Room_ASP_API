using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.PricePerPerson).HasColumnType("decimal(10,2)");
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder.HasOne(x => x.Difficulty)
                   .WithMany(x => x.Rooms)
                   .HasForeignKey(x => x.DifficultyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
