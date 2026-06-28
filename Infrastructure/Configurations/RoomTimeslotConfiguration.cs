using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Configurations
{
    public class RoomTimeslotConfiguration : IEntityTypeConfiguration<RoomTimeslot>
    {
        public void Configure(EntityTypeBuilder<RoomTimeslot> builder)
        {
            builder.HasKey(x => new { x.RoomId, x.TimeslotId });

            builder.HasOne(x => x.Room)
                   .WithMany(x => x.RoomTimeslots)
                   .HasForeignKey(x => x.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Timeslot)
                   .WithMany(x => x.RoomTimeslots)
                   .HasForeignKey(x => x.TimeslotId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
