using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Masses.Locations.Public.GetMassLocations;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.Masses.Locations.Public.GetMassLocationHeadquarters;

public record GetMassLocationHeadquartersQuery() : IQuery<MassLocationPublicResponse>;

public class GetMassLocationHeadquartersQueryHandler(
    IMassLocationRepository _repository) : IQueryHandler<GetMassLocationHeadquartersQuery, MassLocationPublicResponse>
{
    public async Task<Result<MassLocationPublicResponse>> Handle(GetMassLocationHeadquartersQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetMassLocationHeadQuartersSpec();

        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        return Result.Success(new MassLocationPublicResponse(
            massLocation.Id.Value,
            massLocation.Name,
            massLocation.Address,
            massLocation.Latitude,
            massLocation.Longitude,
            massLocation.IsHeadquarters,
            massLocation.MassSchedules.Select(ms => new MassSchedulePublicResponse(
                ms.Day,
                ms.MassTimes.Select(mt => mt.Time).ToList()
            )).ToList()));
    }
}
