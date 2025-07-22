using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public class MassTime : Entity<Guid>
{
    private MassTime(Guid massScheduleId, TimeOnly time)
    {
        MassScheduleId = massScheduleId;
        Time = time;
    }

    // EF Core
    private MassTime() { }

    public Guid MassScheduleId { get; private set; }
    public TimeOnly Time { get; private set; }

    public static MassTime Create(Guid massScheduleId, TimeOnly time)
    {
        return new MassTime(massScheduleId, time);
    }

    public void Update(TimeOnly time)
    {
        Time = time;
    }
}
