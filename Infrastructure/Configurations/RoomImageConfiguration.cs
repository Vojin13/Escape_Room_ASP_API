using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Implementation.Configurations
{
    public class RoomImageConfiguration : IEntityTypeConfiguration<RoomImage>
    {
        public void Configure(EntityTypeBuilder<RoomImage> builder)
        {
            builder.Property(x => x.Path).IsRequired();
            builder.Property(x => x.MimeType).IsRequired().HasMaxLength(50);
            builder.Property(x => x.IsPrimary).HasDefaultValue(false);
            builder.Property(x => x.SortOrder).HasDefaultValue(0);

            builder.HasOne(x => x.Room)
                   .WithMany(x => x.Images)
                   .HasForeignKey(x => x.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
