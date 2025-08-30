namespace Modules.ParishManagement.Application.NewsFolder.Public.GetNews;

public record NewsResponse(
    Guid Id,
    string Title,
    string Content,
    bool Highlight,
    DateTime? HighlightUntil,
    string Summary,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

