using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.IntegrationEvents.PendingMembers;

namespace Modules.Notification.Application.ParishManagement;

public class PendingMemberCreatedIntegrationEventHandler(
    ILogger<PendingMemberCreatedIntegrationEventHandler> _logger
) : IIntegrationEventHandler<PendingMemberCreatedIntegrationEvent>
{
    public Task Handle(PendingMemberCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("PendingMemberCreatedIntegrationEventHandler: {IntegrationEvent}", integrationEvent);
        return Task.CompletedTask;
    }
}
