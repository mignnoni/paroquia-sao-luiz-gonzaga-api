namespace Modules.ParishManagement.Application.OtherSchedules.Public.GetNews;

public record NewsResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<NewsFileResponse> Files);

public record NewsFileResponse(
    string FileName,
    string ContentType,
    string Url);
