using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.UpdateOtherSchedule;

public record UpdateOtherScheduleCommand(
    Guid Id,
    string Title,
    string Content,
    ScheduleType Type) : ICommand;

internal class UpdateOtherScheduleCommandHandler(
    IOtherScheduleRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<UpdateOtherScheduleCommandHandler> _logger) : ICommandHandler<UpdateOtherScheduleCommand>
{
    public async Task<Result> Handle(UpdateOtherScheduleCommand request, CancellationToken cancellationToken)
    {
        var spec = new OtherScheduleByIdSpec(new OtherScheduleId(request.Id));

        var otherSchedule = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (otherSchedule is null)
            return Result.Error("Programação não encontrada");

        var result = otherSchedule.Update(request.Title, request.Content, request.Type);

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Programação atualizada com sucesso: {OtherScheduleId}", otherSchedule.Id);

        return Result.Success();
    }
}
