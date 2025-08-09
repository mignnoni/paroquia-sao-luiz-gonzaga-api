using Ardalis.Result;
using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public class MassSchedule : Entity<Guid>
{
    private MassSchedule(Guid id, MassLocationId massLocationId, string day) : base(id)
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

    public static MassSchedule Create(Guid id, MassLocationId massLocationId, string day)
    {
        return new MassSchedule(id, massLocationId, day);
    }

    internal Result AddMassTime(TimeOnly massTime)
    {
        if (_massTimes.Any(t => t.Time == massTime))
            return Result.Error($"O horário de missa {massTime} já está cadastrado para o dia {Day}");

        var time = MassTime.Create(Guid.NewGuid(), Id, massTime);

        _massTimes.Add(time);

        return Result.Success();
    }

    internal Result UpdateMassTime(Guid massTimeId, TimeOnly massTime)
    {
        var time = _massTimes.FirstOrDefault(t => t.Id == massTimeId);

        if (time is null)
            return Result.Error($"Horário de missa não encontrado para a programação de missas {Day}");

        time.Update(massTime);

        return Result.Success();
    }

    internal Result RemoveMassTime(Guid massTimeId)
    {
        var time = _massTimes.FirstOrDefault(t => t.Id == massTimeId);

        if (time is null)
            return Result.Error($"Horário de missa não encontrado para a programação de missas {Day}");

        _massTimes.Remove(time);

        return Result.Success();
    }

    internal void UpdateDay(string day)
    {
        Day = day;
        UpdatedAt = DateTime.UtcNow;
    }
}
