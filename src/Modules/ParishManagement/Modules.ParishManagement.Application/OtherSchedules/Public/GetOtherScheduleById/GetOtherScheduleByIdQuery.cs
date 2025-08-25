using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.OtherSchedules.Public.GetOtherSchedules;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Public.GetOtherScheduleById;

public record GetOtherScheduleByIdQuery(Guid Id) : IQuery<OtherScheduleResponse>;

public class GetOtherScheduleByIdQueryHandler(
    IOtherScheduleRepository _repository) : IQueryHandler<GetOtherScheduleByIdQuery, OtherScheduleResponse>
{
    public async Task<Result<OtherScheduleResponse>> Handle(GetOtherScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            return Result.Error("O ID da programação é obrigatório");

        var spec = new OtherScheduleByIdSpec(new OtherScheduleId(request.Id), true);
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