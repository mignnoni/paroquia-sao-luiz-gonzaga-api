using System;
using Ardalis.Specification;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Specifications;

public class MassLocationByIdSpec : Specification<MassLocation>
{
    public MassLocationByIdSpec(MassLocationId id, bool isReadOnly = false)
    {
        Query
            .Where(x => x.Id == id)
            .AsNoTracking(isReadOnly);
    }
}
