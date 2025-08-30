using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.GetOtherScheduleById;

public record GetOtherScheduleByIdQuery(Guid Id) : IQuery<OtherScheduleByIdResponse>;

public record OtherScheduleByIdResponse(
    Guid Id,
    string Title,
    string Content,
    ScheduleType Type,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public class GetOtherScheduleByIdQueryHandler(
    IOtherScheduleRepository _repository) : IQueryHandler<GetOtherScheduleByIdQuery, OtherScheduleByIdResponse>
{
    public async Task<Result<OtherScheduleByIdResponse>> Handle(GetOtherScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            return Result.Error("O ID da programação é obrigatório");

        var spec = new OtherScheduleByIdSpec(new OtherScheduleId(request.Id), true);
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