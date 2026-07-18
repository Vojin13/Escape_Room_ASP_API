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

            var seededAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new BookingStatusLookup { Id = 1, Name = "Pending", CreatedAt = seededAt },
                new BookingStatusLookup { Id = 2, Name = "Confirmed", CreatedAt = seededAt },
                new BookingStatusLookup { Id = 3, Name = "Cancelled", CreatedAt = seededAt },
                new BookingStatusLookup { Id = 4, Name = "Completed", CreatedAt = seededAt }
            );
        }
    }
}
