using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Masses;

public record MassLocationId(Guid Value) : IEntityId;
