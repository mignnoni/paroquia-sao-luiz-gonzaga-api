using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.ValueObjects;

namespace Modules.ParishManagement.Domain.NewsFolder;

public class NewsFile : Entity<Guid>
{
    private NewsFile(Guid id, NewsId newsId, UploadInfo uploadInfo) : base(id)
    {
        NewsId = newsId;
        UploadInfo = uploadInfo;
    }

    // EF Core
    private NewsFile() { }

    public NewsId NewsId { get; private set; }
    public UploadInfo UploadInfo { get; private set; }

    public static NewsFile Create(Guid id, NewsId newsId, UploadInfo uploadInfo)
    {
        return new NewsFile(id, newsId, uploadInfo);
    }
}
