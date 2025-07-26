using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Schedules.AddScheduleToLocation;

public record AddScheduleToLocationCommand(
    Guid MassLocationId,
    string Day,
    List<TimeOnly> MassTimes) : ICommand;

internal class AddScheduleToLocationCommandHandler(
    IMassLocationRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<AddScheduleToLocationCommandHandler> _logger) : ICommandHandler<AddScheduleToLocationCommand>
{
    public async Task<Result> Handle(AddScheduleToLocationCommand request, CancellationToken cancellationToken)
    {
        var spec = new MassLocationByIdWithSchedulesSpec(new MassLocationId(request.MassLocationId));

        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        var result = massLocation.AddSchedule(request.Day, request.MassTimes);

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Programação de missas adicionada com sucesso para o local de missas {MassLocationId}, dia {Day}",
            request.MassLocationId,
            request.Day);

        return Result.Success();
    }
}