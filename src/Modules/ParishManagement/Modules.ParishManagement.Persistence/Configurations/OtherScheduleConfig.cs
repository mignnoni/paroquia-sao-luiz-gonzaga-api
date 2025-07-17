using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Persistence.Configurations;

public class OtherScheduleConfig : IEntityTypeConfiguration<OtherSchedule>
{
    public void Configure(EntityTypeBuilder<OtherSchedule> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(x => x.Value, value => new OtherScheduleId(value));

        builder
            .Property(x => x.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.Content)
            .IsRequired();

        builder
            .Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();
            
        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);
    }
}
