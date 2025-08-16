using Ardalis.Result;
using BuildingBlocks.Application;
using BuildingBlocks.Domain;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Locations.CreateMassLocation;

public record CreateMassLocationCommand(
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    bool IsHeadquarters,
    List<MassScheduleInput> MassSchedules) : ICommand;

public record MassScheduleInput(
    string Day,
    List<TimeOnly> MassTimes);

internal class CreateMassLocationCommandHandler(
    IMassLocationRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<CreateMassLocationCommandHandler> _logger) : ICommandHandler<CreateMassLocationCommand>
{
    public async Task<Result> Handle(CreateMassLocationCommand request, CancellationToken cancellationToken)
    {
        var result = MassLocation.Create(
            new MassLocationId(
                Guid.NewGuid()), 
                request.Name, 
                request.Address, 
                request.Latitude, 
                request.Longitude, 
                request.IsHeadquarters);

        if (!result.IsSuccess)
            return Result.Error(result.Errors.First());

        var massLocation = result.Value;

        if (massLocation.IsHeadquarters)
        {
            var headquarters = await _repository.FirstOrDefaultAsync(new GetMassLocationHeadQuartersSpec(), cancellationToken);

            headquarters?.SetIsHeadquarters(false);
        }

        foreach (var schedule in request.MassSchedules)
        {
            var addResult = massLocation.AddSchedule(schedule.Day, schedule.MassTimes);

            if (!addResult.IsSuccess)
                return addResult;
        }

        _repository.Add(massLocation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Local de missas criado com sucesso: {MassLocationId}", massLocation.Id);

        return Result.Success();
    }
}
