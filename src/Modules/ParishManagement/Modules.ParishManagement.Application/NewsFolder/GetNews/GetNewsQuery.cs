using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.NewsFolder.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.NewsFolder.GetNews;

public record GetNewsQuery(
    int PageIndex = 0,
    int PageSize = 10) : IQuery<List<NewsResponse>>;

public record NewsResponse(
    Guid Id,
    string Title,
    string Content,
    bool Highlight,
    DateTime? HighlightUntil,
    string Summary,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public class GetNewsQueryHandler(
    INewsRepository _repository) : IQueryHandler<GetNewsQuery, List<NewsResponse>>
{
    public async Task<Result<List<NewsResponse>>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        if (request.PageSize <= 0)
            return Result.Error("O tamanho da página deve ser maior que 0");

        if (request.PageSize > 100)
            return Result.Error("O tamanho da página não pode ser maior que 100");

        if (request.PageIndex < 0)
            return Result.Error("O índice da página deve ser maior ou igual a 0");

        var spec = new NewsSpec(request.PageIndex, request.PageSize);

        var news = await _repository.ListAsync(spec, cancellationToken) ?? [];

        var response = news.Select(s => new NewsResponse(
            s.Id.Value,
            s.Title,
            s.Content,
            s.Highlight,
            s.HighlightUntil,
            s.Summary,
            s.CreatedAt,
            s.UpdatedAt)).ToList() ?? [];

        return Result.Success(response);
    }
}