using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Times.RemoveTimeFromSchedule;

public record RemoveTimeFromScheduleCommand(
    Guid MassLocationId,
    Guid MassScheduleId,
    Guid MassTimeId) : ICommand;

internal class RemoveTimeFromScheduleCommandHandler(
    IMassLocationRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<RemoveTimeFromScheduleCommandHandler> _logger) : ICommandHandler<RemoveTimeFromScheduleCommand>
{
    public async Task<Result> Handle(RemoveTimeFromScheduleCommand request, CancellationToken cancellationToken)
    {
        var spec = new MassLocationByIdWithIncludesSpec(
            new MassLocationId(request.MassLocationId),
            request.MassScheduleId,
            request.MassTimeId);

        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        var result = massLocation.RemoveTimeFromSchedule(request.MassScheduleId, request.MassTimeId);

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Horário de missa removido com sucesso para o local de missas {MassLocationId}, programação de missas {MassScheduleId}, horário {MassTimeId}",
            request.MassLocationId,
            request.MassScheduleId,
            request.MassTimeId);

        return Result.Success();
    }
}