using Ardalis.Specification;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Specifications;

public class MassLocationsReadOnlySpec : Specification<MassLocation>
{
    public MassLocationsReadOnlySpec(int pageIndex, int pageSize)
    {
        Query
            .AsNoTracking()
            .OrderByDescending(x => x.IsHeadquarters)
            .ThenBy(x => x.Name)
            .Skip(pageIndex * pageSize)
            .Take(pageSize);
    }
}
