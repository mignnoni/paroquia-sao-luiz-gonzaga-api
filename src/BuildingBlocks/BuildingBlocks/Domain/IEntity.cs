namespace BuildingBlocks.Domain;

public interface IEntity
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
