using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Configurations
{
    public class BookingStatusConfiguration : IEntityTypeConfiguration<BookingStatusLookup>
    {
        public void Configure(EntityTypeBuilder<BookingStatusLookup> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.HasData(
                new BookingStatusLookup { Id = 1, Name = "Pending", CreatedAt = DateTime.UtcNow },
                new BookingStatusLookup { Id = 2, Name = "Confirmed", CreatedAt = DateTime.UtcNow },
                new BookingStatusLookup { Id = 3, Name = "Cancelled", CreatedAt = DateTime.UtcNow },
                new BookingStatusLookup { Id = 4, Name = "Completed", CreatedAt = DateTime.UtcNow }
            );
        }
    }
}
