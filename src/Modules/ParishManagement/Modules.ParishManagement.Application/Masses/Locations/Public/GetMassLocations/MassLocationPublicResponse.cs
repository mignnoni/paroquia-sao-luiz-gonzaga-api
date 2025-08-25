using System;

namespace Modules.ParishManagement.Application.Masses.Locations.Public.GetMassLocations;

public record MassLocationPublicResponse(
    Guid Id,
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    bool IsHeadquarters,
    List<MassSchedulePublicResponse> Schedules
);

public record MassSchedulePublicResponse(
    string Day,
    List<TimeOnly> Times
);
