using Ardalis.Specification;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Specifications;

public class OtherSchedulesByTypeSpec : Specification<OtherSchedule>
{
    public OtherSchedulesByTypeSpec(ScheduleType type, bool isReadOnly = false)
    {
        Query
            .Where(x => x.Type == type)
            .OrderByDescending(x => x.CreatedAt)
            .AsNoTracking(isReadOnly);
    }
}