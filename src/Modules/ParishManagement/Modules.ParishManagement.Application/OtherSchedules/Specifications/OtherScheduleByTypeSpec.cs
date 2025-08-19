using System;
using Ardalis.Specification;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Specifications;

public class OtherScheduleByTypeSpec : Specification<OtherSchedule>
{
    public OtherScheduleByTypeSpec(ScheduleType type, bool isReadOnly = false)
    {
        Query
            .Where(x => x.Type == type)
            .AsNoTracking(isReadOnly);
    }
}