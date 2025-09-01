using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.NewsFolder.Public.GetNewsById;
using Modules.ParishManagement.Application.NewsFolder.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.NewsFolder.Public.GetHighlightedNews;

public record GetHighlightedNewsQuery : IQuery<NewsByIdResponse?>;

public class GetHighlightedNewsQueryHandler(
    INewsRepository _repository,
    IS3Service _s3Service) : IQueryHandler<GetHighlightedNewsQuery, NewsByIdResponse?>
{
    public async Task<Result<NewsByIdResponse?>> Handle(GetHighlightedNewsQuery request, CancellationToken cancellationToken)
    {
        var spec = new HighlightedNewsSpec(filterByDate: true, isReadOnly: true);

        var news = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (news is null)
            return Result.Success<NewsByIdResponse?>(null);

        var response = new NewsByIdResponse(
            news.Id.Value,
            news.Title,
            news.Content,
            news.Highlight,
            news.HighlightUntil,
            news.Summary,
            news.CreatedAt,
            news.UpdatedAt,
            news.Files.Select(f => new NewsFileResponse(
                f.UploadInfo.FileName,
                f.UploadInfo.ContentType,
                _s3Service.GetPublicUrl(f.UploadInfo.FileName))).ToList() ?? []);

        return Result.Success<NewsByIdResponse?>(response);
    }
}