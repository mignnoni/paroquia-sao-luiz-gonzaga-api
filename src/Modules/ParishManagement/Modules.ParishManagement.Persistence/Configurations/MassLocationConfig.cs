using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Persistence.Configurations;

public class MassLocationConfig : IEntityTypeConfiguration<MassLocation>
{
    public void Configure(EntityTypeBuilder<MassLocation> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(x => x.Value, value => new MassLocationId(value));

        builder
            .Property(x => x.Name)
            .HasMaxLength(80)
            .IsRequired();

        builder
            .Property(x => x.Address)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Latitude)
            .IsRequired();

        builder 
            .Property(x => x.Longitude)
            .IsRequired();

        builder
            .Property(x => x.IsHeadquarters)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);

        builder
            .HasMany(x => x.MassSchedules)
            .WithOne()
            .HasForeignKey(x => x.MassLocationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
