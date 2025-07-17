using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Persistence.Configurations;

public class MassScheduleConfig : IEntityTypeConfiguration<MassSchedule>
{
    public void Configure(EntityTypeBuilder<MassSchedule> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.Day)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);

        builder
            .HasOne<MassLocation>()
            .WithMany(many => many.MassSchedules)
            .HasForeignKey(x => x.MassLocationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
