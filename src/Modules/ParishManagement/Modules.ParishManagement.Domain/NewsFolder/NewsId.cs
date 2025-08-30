using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.NewsFolder;

public sealed record NewsId(Guid Value) : IEntityId;