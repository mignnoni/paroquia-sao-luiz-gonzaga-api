namespace ParoquiaSLG.API.Modules.ParishManagement.Masses.Contracts;

public record UpdateMassLocationRequest(
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    bool IsHeadquarters);
