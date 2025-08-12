namespace ParoquiaSLG.API.Modules.ParishManagement.Masses.Contracts;

public record CreateMassLocationRequest(
    string Name,
    string Address,
    bool IsHeadquarters,
    List<MassScheduleRequest>? MassSchedules = null);

public record MassScheduleRequest(
    string Day,
    List<TimeOnly> MassTimes);
