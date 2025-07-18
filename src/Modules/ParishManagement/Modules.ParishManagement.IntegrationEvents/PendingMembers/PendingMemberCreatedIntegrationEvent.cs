using System;
using BuildingBlocks.Application.EventBus;

namespace Modules.ParishManagement.IntegrationEvents.PendingMembers;

public sealed record PendingMemberCreatedIntegrationEvent(
    Guid Id,
    DateTime OcurredOn,
    string FullName,
    string Email,
    string Token) : IntegrationEvent(Id, OcurredOn);