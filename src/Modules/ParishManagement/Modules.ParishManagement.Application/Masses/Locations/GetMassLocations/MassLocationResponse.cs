namespace Modules.ParishManagement.Application.Masses.Locations.GetMassLocations;

public record MassLocationResponse(
    Guid Id,
    string Name,
    string Address,
    bool IsHeadquarters
);
