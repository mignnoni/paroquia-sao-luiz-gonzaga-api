namespace ParoquiaSLG.API.Modules.ParishManagement.Masses.Contracts;

public record AddScheduleToLocationRequest(
    string Day,
    List<TimeOnly> MassTimes);
