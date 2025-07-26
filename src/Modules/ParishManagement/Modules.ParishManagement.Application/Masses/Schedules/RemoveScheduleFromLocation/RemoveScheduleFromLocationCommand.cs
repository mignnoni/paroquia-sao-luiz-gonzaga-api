using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Schedules.RemoveScheduleFromLocation;

public record RemoveScheduleFromLocationCommand(
    Guid MassLocationId,
    Guid MassScheduleId) : ICommand;

internal class RemoveScheduleFromLocationCommandHandler(
    IMassLocationRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<RemoveScheduleFromLocationCommandHandler> _logger) : ICommandHandler<RemoveScheduleFromLocationCommand>
{
    public async Task<Result> Handle(RemoveScheduleFromLocationCommand request, CancellationToken cancellationToken)
    {
        var spec = new MassLocationByIdWithSchedulesSpec(new MassLocationId(request.MassLocationId), request.MassScheduleId);

        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        var result = massLocation.RemoveSchedule(request.MassScheduleId);

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Programação de missas removida com sucesso para o local de missas {MassLocationId}, programação {MassScheduleId}",
            request.MassLocationId,
            request.MassScheduleId);

        return Result.Success();
    }
}