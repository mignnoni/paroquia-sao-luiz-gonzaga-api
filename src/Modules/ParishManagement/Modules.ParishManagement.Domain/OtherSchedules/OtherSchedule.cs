using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.OtherSchedules;

public class OtherSchedule : Entity<OtherScheduleId>
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public ScheduleType Type { get; private set; }
}
