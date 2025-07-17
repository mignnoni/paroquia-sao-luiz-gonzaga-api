using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Persistence.Configurations;

public class MassTimeConfig : IEntityTypeConfiguration<MassTime>
{
    public void Configure(EntityTypeBuilder<MassTime> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.Time)
            .HasColumnType("time")
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);
    }
}
