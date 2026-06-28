using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TotalPrice).HasColumnType("decimal(10,2)");

            builder.HasOne(x => x.Room)
                   .WithMany(x => x.Bookings)
                   .HasForeignKey(x => x.RoomId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Bookings)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Timeslot)
                   .WithMany()
                   .HasForeignKey(x => x.TimeslotId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Status)
                   .WithMany(x => x.Bookings)
                   .HasForeignKey(x => x.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
