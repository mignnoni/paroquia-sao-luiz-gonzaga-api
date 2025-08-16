using System;
using Ardalis.Specification;
using Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Specifications;

public class MassLocationByIdReadOnlySpec : Specification<MassLocation, MassLocationByIdResponse>
{
    public MassLocationByIdReadOnlySpec(MassLocationId id)
    {
        Query
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Id == id)
            .Select(x => new MassLocationByIdResponse(
                x.Id.Value,
                x.Name,
                x.Address,
                x.Latitude,
                x.Longitude,
                x.IsHeadquarters,
                x.MassSchedules.Select(y => new MassScheduleResponse(
                    y.Id,
                    y.MassLocationId.Value,
                    y.Day,
                    y.MassTimes.Select(z => new MassTimeResponse(z.Id, z.MassScheduleId, z.Time)).ToList()
                )).ToList()
            ));
    }
}
