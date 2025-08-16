using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.Masses.Locations.GetMassLocations;

public record GetMassLocationsQuery(
    int PageIndex = 0,
    int PageSize = 10) : IQuery<List<MassLocationResponse>>;

public class GetMassLocationsQueryHandler(
    IMassLocationRepository _repository) : IQueryHandler<GetMassLocationsQuery, List<MassLocationResponse>>
{
    public async Task<Result<List<MassLocationResponse>>> Handle(GetMassLocationsQuery request, CancellationToken cancellationToken)
    {
        if (request.PageSize <= 0)
            return Result.Error("O tamanho da página deve ser maior que 0");

        if (request.PageSize > 100)
            return Result.Error("O tamanho da página não pode ser maior que 100");

        if (request.PageIndex < 0)
            return Result.Error("O índice da página deve ser maior ou igual a 0");

        var spec = new MassLocationsReadOnlySpec(request.PageIndex, request.PageSize);

        var massLocations = await _repository.ListAsync(spec, cancellationToken) ?? [];

        return massLocations.Select(s => new MassLocationResponse(
            s.Id.Value,
            s.Name,
            s.Address,
            s.IsHeadquarters)).ToList();
    }
}
