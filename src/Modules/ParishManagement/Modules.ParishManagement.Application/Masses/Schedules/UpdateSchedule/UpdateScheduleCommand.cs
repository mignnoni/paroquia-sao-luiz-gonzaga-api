using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Schedules.UpdateSchedule;

public record UpdateScheduleCommand(
    Guid MassLocationId,
    Guid MassScheduleId,
    string Day) : ICommand;

internal class UpdateScheduleCommandHandler(
    IMassLocationRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<UpdateScheduleCommandHandler> _logger) : ICommandHandler<UpdateScheduleCommand>
{
    public async Task<Result> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        var spec = new MassLocationByIdWithSchedulesSpec(new MassLocationId(request.MassLocationId), request.MassScheduleId);

        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        var result = massLocation.UpdateSchedule(request.MassScheduleId, request.Day);

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Programação de missas atualizada com sucesso para o local de missas {MassLocationId}, programação {MassScheduleId}",
            request.MassLocationId,
            request.MassScheduleId);

        return Result.Success();
    }
}