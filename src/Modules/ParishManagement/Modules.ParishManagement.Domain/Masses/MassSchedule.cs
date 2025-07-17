using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public class MassSchedule : Entity<Guid>
{
    public MassLocationId MassLocationId { get; private set; }
    public string Day { get; private set; }

    private readonly List<MassTime> _massTimes = [];
    public IReadOnlyCollection<MassTime> MassTimes => _massTimes.AsReadOnly();
}
