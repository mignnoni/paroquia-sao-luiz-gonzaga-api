using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Persistence.Configurations;

public class OtherScheduleFileConfig : IEntityTypeConfiguration<OtherScheduleFile>
{
    public void Configure(EntityTypeBuilder<OtherScheduleFile> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.OtherScheduleId)
            .IsRequired();

        builder
            .ComplexProperty(x => x.UploadInfo)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);

        builder
            .HasOne<OtherSchedule>()
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.OtherScheduleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
