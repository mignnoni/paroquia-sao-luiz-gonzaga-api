using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ParishManagement.Domain.NewsFolder;

namespace Modules.ParishManagement.Persistence.Configurations;

public class NewsFileConfig : IEntityTypeConfiguration<NewsFile>
{
    public void Configure(EntityTypeBuilder<NewsFile> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.NewsId)
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
            .HasOne<News>()
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.NewsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
