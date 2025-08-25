using Ardalis.Specification;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Specifications;

public class NewsReadOnlySpec : Specification<OtherSchedule>
{
    public NewsReadOnlySpec(int pageIndex, int pageSize)
    {
        Query
            .Where(x => x.Type == ScheduleType.News)
            .Include(x => x.Files)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .AsNoTracking();
    }
}
