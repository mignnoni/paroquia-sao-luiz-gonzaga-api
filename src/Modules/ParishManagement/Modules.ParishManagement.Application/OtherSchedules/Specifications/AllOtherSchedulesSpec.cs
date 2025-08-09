using Ardalis.Specification;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Specifications;

public class AllOtherSchedulesSpec : Specification<OtherSchedule>
{
    public AllOtherSchedulesSpec(bool isReadOnly = false)
    {
        Query
            .OrderByDescending(x => x.CreatedAt)
            .Include(x => x.Files)
            .AsNoTracking(isReadOnly);
    }
}