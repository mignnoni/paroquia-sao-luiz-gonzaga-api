namespace Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;

public record MassTimeResponse(
    Guid Id,
    TimeOnly Time
);
