using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.NewsFolder.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.NewsFolder;

namespace Modules.ParishManagement.Application.NewsFolder.Public.GetNewsById;

public record GetNewsByIdQuery(Guid Id) : IQuery<NewsByIdResponse>;

public class GetNewsByIdQueryHandler(
    INewsRepository _repository,
    IS3Service _s3Service) : IQueryHandler<GetNewsByIdQuery, NewsByIdResponse>
{
    public async Task<Result<NewsByIdResponse>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new NewsByIdSpec(new NewsId(request.Id), true);

        var news = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (news is null)
            return Result.Error("Notícia não encontrada");

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

        return Result.Success(response);
    }
}