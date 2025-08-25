using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.OtherSchedules.Public.GetOtherSchedules;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Public.GetFirstOtherScheduleByType;

public record GetFirstOtherScheduleByTypeQuery(ScheduleType Type) : IQuery<OtherScheduleResponse>;

public class GetFirstOtherScheduleByTypeQueryHandler(
    IOtherScheduleRepository _repository) : IQueryHandler<GetFirstOtherScheduleByTypeQuery, OtherScheduleResponse>
{
    public async Task<Result<OtherScheduleResponse>> Handle(GetFirstOtherScheduleByTypeQuery request, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(request.Type))
            return Result.Error("O tipo da programação informado é inválido");

        var spec = new OtherScheduleByTypeSpec(request.Type, isReadOnly: true);
        var otherSchedule = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (otherSchedule is null)
            return Result.Error("Programação não encontrada");

        var response = new OtherScheduleResponse(
            otherSchedule.Id.Value,
            otherSchedule.Title,
            otherSchedule.Content,
            otherSchedule.Type);

        return Result.Success(response);
    }
}