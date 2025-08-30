using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.OtherSchedules.GetOtherScheduleById;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.GetFirstOtherScheduleByType;

public record GetFirstOtherScheduleByTypeQuery(ScheduleType Type) : IQuery<OtherScheduleByIdResponse>;

public class GetFirstOtherScheduleByTypeQueryHandler(
    IOtherScheduleRepository _repository) : IQueryHandler<GetFirstOtherScheduleByTypeQuery, OtherScheduleByIdResponse>
{
    public async Task<Result<OtherScheduleByIdResponse>> Handle(GetFirstOtherScheduleByTypeQuery request, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(request.Type))
            return Result.Error("O tipo da programação informado é inválido");

        var spec = new OtherScheduleByTypeSpec(request.Type, isReadOnly: true);
        var otherSchedule = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (otherSchedule is null)
            return Result.Error("Programação não encontrada");

        var response = new OtherScheduleByIdResponse(
            otherSchedule.Id.Value,
            otherSchedule.Title,
            otherSchedule.Content,
            otherSchedule.Type,
            otherSchedule.CreatedAt,
            otherSchedule.UpdatedAt);

        return Result.Success(response);
    }
}