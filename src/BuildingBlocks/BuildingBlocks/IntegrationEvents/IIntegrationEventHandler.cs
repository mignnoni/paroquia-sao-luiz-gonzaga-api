using BuildingBlocks.Domain;
using MediatR;

namespace BuildingBlocks.IntegrationEvents;

public interface IIntegrationEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IIntegrationEvent
{
}
