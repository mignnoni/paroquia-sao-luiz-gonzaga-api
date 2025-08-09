using BuildingBlocks.Application.EventBus;

namespace Modules.IdentityProvider.IntegrationEvents;

public sealed record PasswordResetedIntegrationEvent(
    Guid Id,
    DateTime OcurredOn,
    string FullName,
    string Email) : IntegrationEvent(Id, OcurredOn);
