namespace Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;

public record MassTimeResponse(
    Guid Id,
    Guid MassScheduleId,
    TimeOnly Time
);
