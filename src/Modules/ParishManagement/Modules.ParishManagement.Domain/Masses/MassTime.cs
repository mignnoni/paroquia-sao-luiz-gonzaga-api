using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public class MassTime : Entity<Guid>
{
    private MassTime(Guid id, Guid massScheduleId, TimeOnly time) : base(id)
    {
        MassScheduleId = massScheduleId;
        Time = time;
    }

    // EF Core
    private MassTime() { }

    public Guid MassScheduleId { get; private set; }
    public TimeOnly Time { get; private set; }

    public static MassTime Create(Guid id, Guid massScheduleId, TimeOnly time)
    {
        return new MassTime(id, massScheduleId, time);
    }

    public void Update(TimeOnly time)
    {
        Time = time;
        UpdatedAt = DateTime.UtcNow;
    }
}
