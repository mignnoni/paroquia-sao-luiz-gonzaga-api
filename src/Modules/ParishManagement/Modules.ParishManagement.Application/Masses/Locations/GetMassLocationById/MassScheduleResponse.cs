namespace Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;

public record MassScheduleResponse(
    Guid Id,
    string Day,
    List<MassTimeResponse> MassTimes
);