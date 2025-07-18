using BuildingBlocks.Application.EventBus;
using Modules.ParishManagement.IntegrationEvents.PendingMembers;

namespace Modules.Notification.Application.ParishManagement;

public class PendingMemberCreatedIntegrationEventHandler : IIntegrationEventHandler<PendingMemberCreatedIntegrationEvent>
{
    public Task Handle(PendingMemberCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
