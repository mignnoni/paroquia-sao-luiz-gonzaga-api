using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;
using Modules.ParishManagement.Application.Masses.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.Masses.Locations.GetMassLocationHeadquarters;

public record GetMassLocationHeadquartersQuery() : IQuery<MassLocationByIdResponse>;

public class GetMassLocationHeadquartersQueryHandler(
    IMassLocationRepository _repository) : IQueryHandler<GetMassLocationHeadquartersQuery, MassLocationByIdResponse>
{
    public async Task<Result<MassLocationByIdResponse>> Handle(GetMassLocationHeadquartersQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetMassLocationHeadQuartersSpec();

        var massLocation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (massLocation is null)
            return Result.Error("Local de missas não encontrado");

        return Result.Success(new MassLocationByIdResponse(
            massLocation.Id.Value,
            massLocation.Name,
            massLocation.Address,
            massLocation.Latitude,
            massLocation.Longitude,
            massLocation.IsHeadquarters,
            massLocation.MassSchedules.Select(s => new MassScheduleResponse(
                s.Id,
                s.MassLocationId.Value,
                s.Day,
                s.MassTimes.Select(t => new MassTimeResponse(t.Id, t.MassScheduleId, t.Time)).ToList())).ToList()));
    }
}
