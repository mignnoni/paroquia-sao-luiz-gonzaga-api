using Ardalis.Specification;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Specifications;

public class MassLocationsReadOnlySpec : Specification<MassLocation>
{
    public MassLocationsReadOnlySpec(int pageIndex, int pageSize, bool includeRelatedEntities = false)
    {
        Query
            .AsNoTracking()
            .OrderByDescending(x => x.IsHeadquarters)
            .ThenBy(x => x.Name);

        if (includeRelatedEntities)
        {
            Query
                .AsSplitQuery()
                .Include(i => i.MassSchedules)
                .ThenInclude(ii => ii.MassTimes);
        }

        Query
            .Skip(pageIndex * pageSize)
            .Take(pageSize);
    }
}
