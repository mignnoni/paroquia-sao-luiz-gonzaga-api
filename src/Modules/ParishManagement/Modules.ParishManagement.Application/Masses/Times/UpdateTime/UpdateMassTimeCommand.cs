using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Times.UpdateTime;

public record UpdateMassTimeCommand(
    Guid MassLocationId,
    Guid MassScheduleId,
    Guid MassTimeId,
    TimeOnly MassTime) : ICommand;

internal class UpdateMassTimeCommandHandler(
    IMassLocationRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<UpdateMassTimeCommandHandler> _logger) : ICommandHandler<UpdateMassTimeCommand>
{
    public async Task<Result> Handle(UpdateMassTimeCommand request, CancellationToken cancellationToken)
    {
        var spec = new MassLocationByIdWithIncludesSpec(
            new MassLocationId(request.MassLocationId),
            request.MassScheduleId,
            request.MassTimeId);

        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        var result = massLocation.UpdateMassTime(request.MassScheduleId, request.MassTimeId, request.MassTime);

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Horário de missa atualizado com sucesso para o local de missas {MassLocationId}, programação de missas {MassScheduleId} e horário {MassTimeId}",
            request.MassLocationId,
            request.MassScheduleId,
            request.MassTimeId);

        return Result.Success();
    }
}
