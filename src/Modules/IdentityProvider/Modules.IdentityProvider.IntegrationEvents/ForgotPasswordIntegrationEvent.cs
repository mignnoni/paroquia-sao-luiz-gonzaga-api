using BuildingBlocks.Application.EventBus;

namespace Modules.IdentityProvider.IntegrationEvents;

public sealed record ForgotPasswordIntegrationEvent(
    Guid Id,
    DateTime OcurredOn,
    string FullName,
    string Email,
    string Token) : IntegrationEvent(Id, OcurredOn);