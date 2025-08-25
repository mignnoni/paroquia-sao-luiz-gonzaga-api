using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Public.GetOtherSchedules;

public record GetOtherSchedulesQuery(ScheduleType Type) : IQuery<List<OtherScheduleResponse>>;

public class GetOtherSchedulesQueryHandler(
    IOtherScheduleRepository _repository) : IQueryHandler<GetOtherSchedulesQuery, List<OtherScheduleResponse>>
{
    public async Task<Result<List<OtherScheduleResponse>>> Handle(GetOtherSchedulesQuery request, CancellationToken cancellationToken)
    {
        var spec = new AllOtherSchedulesSpec(pageIndex: 0, pageSize: 100, type: request.Type, isReadOnly: true);

        var otherSchedules = await _repository.ListAsync(spec, cancellationToken) ?? [];

        var response = otherSchedules.Select(s => new OtherScheduleResponse(
            s.Id.Value,
            s.Title,
            s.Content,
            s.Type)).ToList() ?? [];

        return Result.Success(response);
    }
}