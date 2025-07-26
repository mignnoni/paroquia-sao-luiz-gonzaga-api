using Ardalis.Specification;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Application.Masses.Specifications;

public class MassLocationByIdWithSchedulesSpec : Specification<MassLocation>
{
    public MassLocationByIdWithSchedulesSpec(MassLocationId id, Guid? massScheduleId = null, bool isReadOnly = false)
    {
        Query
            .Where(x => x.Id == id)
            .AsSplitQuery()
            .AsNoTracking(isReadOnly);

        if (massScheduleId is not null)
        {
            Query
                .Include(x => x.MassSchedules.Where(y => y.Id == massScheduleId));
        }
        else
        {
            Query
                .Include(x => x.MassSchedules);
        }
    }
}