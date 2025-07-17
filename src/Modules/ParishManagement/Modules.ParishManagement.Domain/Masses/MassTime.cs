using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public class MassTime : Entity<Guid>
{
    public TimeOnly Time { get; private set; }
}
