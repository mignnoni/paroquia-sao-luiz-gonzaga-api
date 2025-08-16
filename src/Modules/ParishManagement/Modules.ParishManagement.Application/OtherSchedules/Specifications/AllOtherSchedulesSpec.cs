using Ardalis.Specification;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Specifications;

public class AllOtherSchedulesSpec : Specification<OtherSchedule>
{
    public AllOtherSchedulesSpec(int pageIndex, int pageSize, ScheduleType? type = null, bool isReadOnly = false)
    {
        Query
            .AsNoTracking(isReadOnly);

        if (type is not null)
        {
            Query
                .Where(x => x.Type == type);
        }

        Query
            .OrderByDescending(x => x.CreatedAt)
            .Skip(pageIndex * pageSize)
            .Take(pageSize);
    }
}