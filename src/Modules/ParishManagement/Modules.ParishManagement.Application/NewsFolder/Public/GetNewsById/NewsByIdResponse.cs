namespace Modules.ParishManagement.Application.NewsFolder.Public.GetNewsById;

public record NewsByIdResponse(
    Guid Id,
    string Title,
    string Content,
    bool Highlight,
    DateTime? HighlightUntil,
    string Summary,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<NewsFileResponse> Files);

public record NewsFileResponse(
    string FileName,
    string ContentType,
    string Url);