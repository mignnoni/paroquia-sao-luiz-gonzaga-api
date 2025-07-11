using MediatR;

namespace BuildingBlocks.Domain
{
    public interface IIntegrationEvent : INotification
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
    }
}
