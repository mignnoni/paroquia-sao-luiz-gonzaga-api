using Ardalis.Specification;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Specifications;

public class OtherScheduleByIdSpec : Specification<OtherSchedule>
{
    public OtherScheduleByIdSpec(OtherScheduleId id, bool isReadOnly = false)
    {
        Query
            .Where(x => x.Id == id)
            .AsNoTracking(isReadOnly);
    }
}