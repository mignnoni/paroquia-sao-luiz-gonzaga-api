namespace Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;

public record MassLocationByIdResponse(
    Guid Id,
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    bool IsHeadquarters,
    List<MassScheduleResponse> MassSchedules
);