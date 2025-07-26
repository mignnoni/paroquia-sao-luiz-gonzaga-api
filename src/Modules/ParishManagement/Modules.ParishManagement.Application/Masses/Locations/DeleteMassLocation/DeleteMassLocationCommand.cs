using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Locations.DeleteMassLocation;

public record DeleteMassLocationCommand(Guid Id) : ICommand;

internal class DeleteMassLocationCommandHandler(
    IMassLocationRepository _repository,
    IUnitOfWork _unitOfWork,
    ILogger<DeleteMassLocationCommandHandler> _logger) : ICommandHandler<DeleteMassLocationCommand>
{
    public async Task<Result> Handle(DeleteMassLocationCommand request, CancellationToken cancellationToken)
    {
        var spec = new MassLocationByIdSpec(new MassLocationId(request.Id));

        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        _repository.Delete(massLocation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Local de missas removido com sucesso: {MassLocationId}",
            request.Id);

        return Result.Success();
    }
}