using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.OtherSchedules.Public.GetNews;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Public.GetNewsById;

public record GetNewsByIdQuery(Guid Id) : IQuery<NewsResponse>;

public class GetNewsByIdQueryHandler(
    IOtherScheduleRepository _repository,
    IS3Service _s3Service) : IQueryHandler<GetNewsByIdQuery, NewsResponse>
{
    public async Task<Result<NewsResponse>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new OtherScheduleByIdSpec(new OtherScheduleId(request.Id), true);

        var news = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (news is null)
            return Result.Error("Notícia não encontrada");

        var response = new NewsResponse(
            news.Id.Value,
            news.Title,
            news.Content,
            news.CreatedAt,
            news.UpdatedAt,
            news.Files.Select(f => new NewsFileResponse(
                f.UploadInfo.FileName,
                f.UploadInfo.ContentType,
                _s3Service.GetPublicUrl(f.UploadInfo.FileName))).ToList());

        return Result.Success(response);
    }
}