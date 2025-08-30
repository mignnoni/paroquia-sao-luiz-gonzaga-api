using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.CreateOtherSchedule;

public record CreateOtherScheduleCommand(
    string Title,
    string Content,
    ScheduleType Type) : ICommand;

internal class CreateOtherScheduleCommandHandler(
    IOtherScheduleRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<CreateOtherScheduleCommandHandler> _logger) : ICommandHandler<CreateOtherScheduleCommand>
{
    public async Task<Result> Handle(CreateOtherScheduleCommand request, CancellationToken cancellationToken)
    {
        var result = OtherSchedule.Create(
            new OtherScheduleId(Guid.NewGuid()),
            request.Title,
            request.Content,
            request.Type);

        if (!result.IsSuccess)
        {
            return Result.Error(result.Errors.First());
        }

        var otherSchedule = result.Value;

        _repository.Add(otherSchedule);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Programação criada com sucesso: {OtherScheduleId}", otherSchedule.Id);

        return Result.Success();
    }
}