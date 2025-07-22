using Ardalis.Result;
using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public class MassSchedule : Entity<Guid>
{
    private MassSchedule(MassLocationId massLocationId, string day)
    {
        MassLocationId = massLocationId;
        Day = day;
    }

    // EF Core
    private MassSchedule() { }

    public MassLocationId MassLocationId { get; private set; }
    public string Day { get; private set; }

    private readonly List<MassTime> _massTimes = [];
    public IReadOnlyCollection<MassTime> MassTimes => _massTimes.AsReadOnly();

    public static MassSchedule Create(MassLocationId massLocationId, string day)
    {
        return new MassSchedule(massLocationId, day);
    }

    internal Result AddMassTime(TimeOnly massTime)
    {
        if (_massTimes.Any(t => t.Time == massTime))
            return Result.Error($"O horário de missa {massTime} já está cadastrado para o dia {Day}");

        var time = MassTime.Create(Id, massTime);

        _massTimes.Add(time);

        return Result.Success();
    }
}
