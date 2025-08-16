using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.GetOtherSchedules;

public record GetOtherSchedulesQuery(
    int PageIndex = 0,
    int PageSize = 10,
    ScheduleType? Type = null) : IQuery<List<OtherScheduleResponse>>;

public record OtherScheduleResponse(
    Guid Id,
    string Title,
    string Content,
    ScheduleType Type,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public class GetOtherSchedulesQueryHandler(
    IOtherScheduleRepository _repository) : IQueryHandler<GetOtherSchedulesQuery, List<OtherScheduleResponse>>
{
    public async Task<Result<List<OtherScheduleResponse>>> Handle(GetOtherSchedulesQuery request, CancellationToken cancellationToken)
    {
        if (request.PageSize <= 0)
            return Result.Error("O tamanho da página deve ser maior que 0");

        if (request.PageSize > 100)
            return Result.Error("O tamanho da página não pode ser maior que 100");

        if (request.PageIndex < 0)
            return Result.Error("O índice da página deve ser maior ou igual a 0");

        var spec = new AllOtherSchedulesSpec(request.PageIndex, request.PageSize, request.Type, isReadOnly: true);

        var otherSchedules = await _repository.ListAsync(spec, cancellationToken) ?? [];

        var response = otherSchedules.Select(s => new OtherScheduleResponse(
            s.Id.Value,
            s.Title,
            s.Content,
            s.Type,
            s.CreatedAt,
            s.UpdatedAt)).ToList() ?? [];

        return Result.Success(response);
    }
}