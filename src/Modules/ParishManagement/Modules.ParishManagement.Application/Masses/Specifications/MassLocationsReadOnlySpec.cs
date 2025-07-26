using Ardalis.Specification;
using Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;
using Modules.ParishManagement.Application.Masses.Locations.GetMassLocations;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Specifications;

public class MassLocationsReadOnlySpec : Specification<MassLocation, MassLocationResponse>
{
    public MassLocationsReadOnlySpec(int pageIndex, int pageSize)
    {
        Query
            .AsNoTracking()
            .AsSplitQuery()
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(x => new MassLocationResponse(
                x.Id.Value,
                x.Name,
                x.Address,
                x.IsHeadquarters,
                x.MassSchedules.Select(y => new MassScheduleResponse(
                    y.Id,
                    y.Day,
                    y.MassTimes.Select(z => new MassTimeResponse(z.Id, z.Time)).ToList()
                )).ToList()
            ));
    }
}
