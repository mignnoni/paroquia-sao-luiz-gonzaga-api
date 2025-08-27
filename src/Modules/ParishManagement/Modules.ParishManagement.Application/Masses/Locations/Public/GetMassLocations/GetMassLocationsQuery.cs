using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.Masses.Locations.Public.GetMassLocations;

public record GetMassLocationsQuery(
    int PageIndex = 0,
    int PageSize = 25) : IQuery<List<MassLocationPublicResponse>>;

public class GetMassLocationsQueryHandler(
    IMassLocationRepository _repository) : IQueryHandler<GetMassLocationsQuery, List<MassLocationPublicResponse>>
{
    public async Task<Result<List<MassLocationPublicResponse>>> Handle(GetMassLocationsQuery request, CancellationToken cancellationToken)
    {
        if (request.PageSize <= 0)
            return Result.Error("O tamanho da página deve ser maior que 0");

        if (request.PageSize > 100)
            return Result.Error("O tamanho da página não pode ser maior que 100");

        if (request.PageIndex < 0)
            return Result.Error("O índice da página deve ser maior ou igual a 0");

        var spec = new MassLocationsReadOnlySpec(request.PageIndex, request.PageSize, includeRelatedEntities: true);

        var massLocations = await _repository.ListAsync(spec, cancellationToken) ?? [];

        return massLocations.Select(s => new MassLocationPublicResponse(
            s.Id.Value,
            s.Name,
            s.Address,
            s.Latitude,
            s.Longitude,
            s.IsHeadquarters,
            s.MassSchedules.Select(ms => new MassSchedulePublicResponse(
                ms.Day,
                ms.MassTimes.Select(mt => mt.Time).ToList()
            )).ToList())).ToList();
    }
}
