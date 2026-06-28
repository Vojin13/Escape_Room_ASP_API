using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RoleUseCaseConfiguration : IEntityTypeConfiguration<RoleUseCase>
    {
        public void Configure(EntityTypeBuilder<RoleUseCase> builder)
        {
            builder.HasKey(x => new { x.RoleId, x.UseCaseId });

            builder.HasOne(x => x.Role)
                   .WithMany(x => x.RoleUseCases)
                   .HasForeignKey(x => x.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
