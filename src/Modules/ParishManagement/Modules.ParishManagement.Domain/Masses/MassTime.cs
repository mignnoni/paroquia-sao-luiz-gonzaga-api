using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public class MassTime : Entity<Guid>
{
    public Guid MassScheduleId { get; private set; }
    public TimeOnly Time { get; private set; }
}
