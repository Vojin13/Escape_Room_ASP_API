using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Configurations
{
    public class RoomBlockadeConfiguration : IEntityTypeConfiguration<RoomBlockade>
    {
        public void Configure(EntityTypeBuilder<RoomBlockade> builder)
        {
            builder.Property(x => x.Reason).IsRequired().HasMaxLength(500);

            builder.HasOne(x => x.Room)
                   .WithMany(x => x.Blockades)
                   .HasForeignKey(x => x.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Timeslot)
                   .WithMany()
                   .HasForeignKey(x => x.TimeslotId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
