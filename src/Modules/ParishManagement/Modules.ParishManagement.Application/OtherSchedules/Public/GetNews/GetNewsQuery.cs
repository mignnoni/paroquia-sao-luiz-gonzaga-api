using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.OtherSchedules.Public.GetNews;

public record GetNewsQuery(
    int PageIndex = 0,
    int PageSize = 10) : IQuery<List<NewsResponse>>;

public class GetNewsQueryHandler(
    IOtherScheduleRepository _repository,
    IS3Service _s3Service) : IQueryHandler<GetNewsQuery, List<NewsResponse>>
{
    public async Task<Result<List<NewsResponse>>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        if (request.PageSize <= 0)
            return Result.Error("O tamanho da página deve ser maior que 0");

        if (request.PageSize > 100)
            return Result.Error("O tamanho da página não pode ser maior que 100");

        if (request.PageIndex < 0)
            return Result.Error("O índice da página deve ser maior ou igual a 0");

        var spec = new NewsReadOnlySpec(request.PageIndex, request.PageSize);

        var news = await _repository.ListAsync(spec, cancellationToken) ?? [];

        var response = news.Select(s => new NewsResponse(
            s.Id.Value,
            s.Title,
            s.Content,
            s.CreatedAt,
            s.UpdatedAt,
            s.Files.Select(f => new NewsFileResponse(
                f.UploadInfo.FileName,
                f.UploadInfo.ContentType,
                _s3Service.GetPublicUrl(f.UploadInfo.FileName))).ToList())).ToList() ?? [];

        return Result.Success(response);
    }
}