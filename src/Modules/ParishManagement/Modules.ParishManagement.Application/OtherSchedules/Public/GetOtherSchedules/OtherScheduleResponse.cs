using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.Public.GetOtherSchedules;

public record OtherScheduleResponse(
    Guid Id,
    string Title,
    string Content,
    ScheduleType Type);
