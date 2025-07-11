using BuildingBlocks.Domain;
using MediatR;

namespace BuildingBlocks.Application;

public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}

