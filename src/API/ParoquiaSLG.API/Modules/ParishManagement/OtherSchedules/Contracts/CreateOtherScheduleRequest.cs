using Modules.ParishManagement.Domain.OtherSchedules;

namespace ParoquiaSLG.API.Modules.ParishManagement.OtherSchedules.Contracts;

public record CreateOtherScheduleRequest(
    string Title,
    string Content,
    ScheduleType Type);
