using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ParishManagement.Domain.NewsFolder;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Persistence.Configurations;

public class NewsConfig : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(x => x.Value, value => new NewsId(value));

        builder
            .Property(x => x.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder
            .Property(x => x.Content)
            .IsRequired();

        builder
            .Property(x => x.Summary)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Highlight)
            .IsRequired();

        builder
            .Property(x => x.HighlightUntil);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);
    }
}
