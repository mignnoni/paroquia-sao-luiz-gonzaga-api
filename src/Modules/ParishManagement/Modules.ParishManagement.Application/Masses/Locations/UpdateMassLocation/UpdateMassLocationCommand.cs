using Ardalis.Result;
using BuildingBlocks.Application;
using BuildingBlocks.Domain;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Locations.UpdateMassLocation;

public record UpdateMassLocationCommand(
    Guid Id,
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    bool IsHeadquarters) : ICommand;

internal class UpdateMassLocationCommandHandler(
    IMassLocationRepository _repository,
    IUnitOfWork _unitOfWork) : ICommandHandler<UpdateMassLocationCommand>
{
    public async Task<Result> Handle(UpdateMassLocationCommand request, CancellationToken cancellationToken)
    {
        var spec = new MassLocationByIdSpec(new MassLocationId(request.Id));

        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        if (!massLocation.IsHeadquarters && request.IsHeadquarters)
        {
            var headquarters = await _repository.FirstOrDefaultAsync(new GetMassLocationHeadQuartersSpec(), cancellationToken);

            headquarters?.SetIsHeadquarters(false);
        }

        var result = massLocation.Update(
            request.Name, 
            request.Address,
            request.Latitude,
            request.Longitude,
            request.IsHeadquarters);

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

