using Ardalis.Specification;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Specifications;

public class MassLocationByIdWithIncludesSpec : Specification<MassLocation>
{
    public MassLocationByIdWithIncludesSpec(MassLocationId id, Guid? massScheduleId = null, Guid? massTimeId = null, bool isReadOnly = false)
    {
        Query
            .Where(x => x.Id == id)
            .AsSplitQuery()
            .AsNoTracking(isReadOnly);

        if (massScheduleId is not null && massTimeId is not null)
        {
            Query
                .Include(x => x.MassSchedules.Where(y => y.Id == massScheduleId))
                .ThenInclude(x => x.MassTimes.Where(y => y.Id == massTimeId));
        }
        else if (massScheduleId is not null)
        {
            Query
                .Include(x => x.MassSchedules.Where(y => y.Id == massScheduleId))
                .ThenInclude(x => x.MassTimes);
        }
        else
        {
            Query
                .Include(x => x.MassSchedules)
                .ThenInclude(x => x.MassTimes);
        }
    }
}
