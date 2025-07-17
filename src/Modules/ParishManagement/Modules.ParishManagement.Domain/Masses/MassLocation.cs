using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public class MassLocation : Entity<MassLocationId>
{
    public string Name { get; private set; }
    public string Address { get; private set; }
    public bool IsHeadquarters { get; private set; }

    private readonly List<MassSchedule> _massSchedules = [];
    public IReadOnlyCollection<MassSchedule> MassSchedules => _massSchedules.AsReadOnly();
}
