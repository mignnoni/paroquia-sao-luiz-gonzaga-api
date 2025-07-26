using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Times.AddTimeToSchedule;

public record AddTimeToScheduleCommand(
    Guid MassLocationId,
    Guid MassScheduleId,
    TimeOnly MassTime) : ICommand;

internal class AddTimeToScheduleCommandHandler(
    IMassLocationRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<AddTimeToScheduleCommandHandler> _logger) : ICommandHandler<AddTimeToScheduleCommand>
{
    public async Task<Result> Handle(AddTimeToScheduleCommand request, CancellationToken cancellationToken)
    {
        var spec = new MassLocationByIdWithIncludesSpec(
            new MassLocationId(request.MassLocationId),
            request.MassScheduleId);

        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        var result = massLocation.AddTimeToSchedule(request.MassScheduleId, request.MassTime);

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Horário de missa adicionado com sucesso para o local de missas {MassLocationId}, programação de missas {MassScheduleId}",
            request.MassLocationId,
            request.MassScheduleId);

        return Result.Success();
    }
}