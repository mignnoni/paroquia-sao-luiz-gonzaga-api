using Ardalis.Result;
using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public class MassLocation : Entity<MassLocationId>
{
    private MassLocation(string name, string address, bool isHeadquarters)
    {
        Name = name;
        Address = address;
        IsHeadquarters = isHeadquarters;
    }

    // EF Core
    private MassLocation() { }

    public string Name { get; private set; }
    public string Address { get; private set; }
    public bool IsHeadquarters { get; private set; }

    private readonly List<MassSchedule> _massSchedules = [];
    public IReadOnlyCollection<MassSchedule> MassSchedules => _massSchedules.AsReadOnly();

    public static Result<MassLocation> Create(string name, string address, bool isHeadquarters)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Error("Nome da localização é obrigatório");

        if (string.IsNullOrWhiteSpace(address))
            return Result.Error("Endereço da localização é obrigatório");

        return Result.Success(new MassLocation(name, address, isHeadquarters));
    }

    public Result Update(string name, string address, bool isHeadquarters)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Error("Nome da localização é obrigatório");

        if (string.IsNullOrWhiteSpace(address))
            return Result.Error("Endereço da localização é obrigatório");

        Name = name;
        Address = address;
        IsHeadquarters = isHeadquarters;

        return Result.Success();
    }

    public Result AddSchedule(string day, List<TimeOnly> massTimes)
    {
        if (string.IsNullOrWhiteSpace(day))
            return Result.Error("É obrigatório informar o dia da programação de missas");

        if (_massSchedules.Any(s => s.Day == day))
            return Result.Error($"Já existe uma programação de missas para {day}");

        var schedule = MassSchedule.Create(Id, day);

        foreach (var time in massTimes)
        {
            var result = schedule.AddMassTime(time);

            if (!result.IsSuccess)
                return result;
        }

        _massSchedules.Add(schedule);

        return Result.Success();
    }

    public void SetIsHeadquarters(bool isHeadquarters)
    {
        if (IsHeadquarters == isHeadquarters)
            return;

        IsHeadquarters = isHeadquarters;
        UpdatedAt = DateTime.UtcNow;
    }
}
