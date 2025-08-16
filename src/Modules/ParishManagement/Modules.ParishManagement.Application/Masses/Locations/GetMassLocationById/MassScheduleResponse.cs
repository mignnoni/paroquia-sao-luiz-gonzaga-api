namespace Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;

public record MassScheduleResponse(
    Guid Id,
    Guid MassLocationId,
    string Day,
    List<MassTimeResponse> MassTimes
);