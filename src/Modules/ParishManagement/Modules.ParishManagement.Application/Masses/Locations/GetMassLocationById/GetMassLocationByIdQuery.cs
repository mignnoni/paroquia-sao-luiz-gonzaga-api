using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;

public record GetMassLocationByIdQuery(
    Guid Id) : IQuery<MassLocationByIdResponse>;

public class GetMassLocationByIdQueryHandler(
    IMassLocationRepository _repository) : IQueryHandler<GetMassLocationByIdQuery, MassLocationByIdResponse>
{
    public async Task<Result<MassLocationByIdResponse>> Handle(GetMassLocationByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            return Result.Error("O ID do local de missas é obrigatório");

        var spec = new MassLocationByIdReadOnlySpec(new MassLocationId(request.Id));
        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        return Result.Success(massLocation);
    }
}
