using System;
using Ardalis.Specification;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Specifications;

public class GetMassLocationHeadQuartersSpec : Specification<MassLocation>
{
    public GetMassLocationHeadQuartersSpec(bool isReadOnly = false)
    {
        Query
            .Where(x => x.IsHeadquarters)
            .AsNoTracking(isReadOnly);
    }
}
