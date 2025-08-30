using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.DeleteOtherSchedule;

public record DeleteOtherScheduleCommand(Guid Id) : ICommand;

internal class DeleteOtherScheduleCommandHandler(
    IOtherScheduleRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<DeleteOtherScheduleCommandHandler> _logger) : ICommandHandler<DeleteOtherScheduleCommand>
{
    public async Task<Result> Handle(DeleteOtherScheduleCommand request, CancellationToken cancellationToken)
    {
        var spec = new OtherScheduleByIdSpec(new OtherScheduleId(request.Id));

        var otherSchedule = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (otherSchedule is null)
            return Result.Error("Programação não encontrada");

        _repository.Delete(otherSchedule);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Programação removida com sucesso: {OtherScheduleId}",
            request.Id);

        return Result.Success();
    }
}