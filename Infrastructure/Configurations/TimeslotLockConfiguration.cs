using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Configurations
{
    public class TimeslotLockConfiguration : IEntityTypeConfiguration<TimeslotLock>
    {
        public void Configure(EntityTypeBuilder<TimeslotLock> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Room)
                   .WithMany()
                   .HasForeignKey(x => x.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Timeslot)
                   .WithMany(x => x.TimeslotLocks)
                   .HasForeignKey(x => x.TimeslotId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.TimeslotLocks)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
