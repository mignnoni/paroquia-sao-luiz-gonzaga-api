namespace Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;

public record MassLocationByIdResponse(
    Guid Id,
    string Name,
    string Address,
    bool IsHeadquarters,
    List<MassScheduleResponse> MassSchedules
);