using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.NewsFolder.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.NewsFolder;

namespace Modules.ParishManagement.Application.NewsFolder.GetNewsById;

public record GetNewsByIdQuery(Guid Id) : IQuery<NewsByIdResponse>;

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
    Guid Id,
    string Name,
    string ContentType,
    string Url);

public class GetNewsByIdQueryHandler(
    INewsRepository _repository,
    IS3Service _s3Service) : IQueryHandler<GetNewsByIdQuery, NewsByIdResponse>
{
    public async Task<Result<NewsByIdResponse>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            return Result.Error("O ID da notícia é obrigatório");

        var spec = new NewsByIdSpec(new NewsId(request.Id), true);
        var news = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (news is null)
            return Result.Error("Notícia não encontrada");

        var files = news.Files.Select(f => new NewsFileResponse(
            f.Id,
            f.UploadInfo.FileName,
            f.UploadInfo.ContentType,
            _s3Service.GetPublicUrl(f.UploadInfo.FileName))).ToList() ?? [];

        var response = new NewsByIdResponse(
            news.Id.Value,
            news.Title,
            news.Content,
            news.Highlight,
            news.HighlightUntil,
            news.Summary,
            news.CreatedAt,
            news.UpdatedAt,
            files);

        return Result.Success(response);
    }
}