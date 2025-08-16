using Ardalis.Result;
using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public class MassLocation : Entity<MassLocationId>
{
    private MassLocation(MassLocationId id, string name, string address, double latitude, double longitude, bool isHeadquarters) : base(id)
    {
        Name = name;
        Address = address;
        IsHeadquarters = isHeadquarters;
        Latitude = latitude;
        Longitude = longitude;
    }

    // EF Core
    private MassLocation() { }

    public string Name { get; private set; }
    public string Address { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public bool IsHeadquarters { get; private set; }

    private readonly List<MassSchedule> _massSchedules = [];
    public IReadOnlyCollection<MassSchedule> MassSchedules => _massSchedules.AsReadOnly();

    public static Result<MassLocation> Create(MassLocationId id, string name, string address, double latitude, double longitude, bool isHeadquarters)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Error("Nome da localização é obrigatório");

        if (string.IsNullOrWhiteSpace(address))
            return Result.Error("Endereço da localização é obrigatório");

        return Result.Success(new MassLocation(id, name, address, latitude, longitude, isHeadquarters));
    }

    public Result Update(string name, string address, double latitude, double longitude, bool isHeadquarters)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Error("Nome da localização é obrigatório");

        if (string.IsNullOrWhiteSpace(address))
            return Result.Error("Endereço da localização é obrigatório");

        Name = name;
        Address = address;
        IsHeadquarters = isHeadquarters;
        Latitude = latitude;
        Longitude = longitude;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result AddSchedule(string day, List<TimeOnly> massTimes)
    {
        if (string.IsNullOrWhiteSpace(day))
            return Result.Error("É obrigatório informar o dia da programação de missas");

        if (_massSchedules.Any(s => s.Day == day))
            return Result.Error($"Já existe uma programação de missas para {day}");

        var schedule = MassSchedule.Create(Guid.NewGuid(), Id, day);

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

    public Result UpdateMassTime(Guid massScheduleId, Guid massTimeId, TimeOnly massTime)
    {
        var schedule = _massSchedules.FirstOrDefault(s => s.Id == massScheduleId);

        if (schedule is null)
            return Result.Error("Programação de missas não encontrada");

        return schedule.UpdateMassTime(massTimeId, massTime);
    }

    public Result AddTimeToSchedule(Guid massScheduleId, TimeOnly massTime)
    {
        var schedule = _massSchedules.FirstOrDefault(s => s.Id == massScheduleId);

        if (schedule is null)
            return Result.Error("Programação de missas não encontrada");

        return schedule.AddMassTime(massTime);
    }

    public Result RemoveTimeFromSchedule(Guid massScheduleId, Guid massTimeId)
    {
        var schedule = _massSchedules.FirstOrDefault(s => s.Id == massScheduleId);

        if (schedule is null)
            return Result.Error("Programação de missas não encontrada");

        return schedule.RemoveMassTime(massTimeId);
    }

    public Result RemoveSchedule(Guid massScheduleId)
    {
        var schedule = _massSchedules.FirstOrDefault(s => s.Id == massScheduleId);

        if (schedule is null)
            return Result.Error("Programação de missas não encontrada");

        _massSchedules.Remove(schedule);

        return Result.Success();
    }

    public Result UpdateSchedule(Guid massScheduleId, string day)
    {
        if (string.IsNullOrWhiteSpace(day))
            return Result.Error("É obrigatório informar o dia da programação de missas");

        var schedule = _massSchedules.FirstOrDefault(s => s.Id == massScheduleId);

        if (schedule is null)
            return Result.Error("Programação de missas não encontrada");

        if (_massSchedules.Any(s => s.Id != massScheduleId && s.Day == day))
            return Result.Error($"Já existe uma programação de missas para {day}");

        schedule.UpdateDay(day);

        return Result.Success();
    }
}
