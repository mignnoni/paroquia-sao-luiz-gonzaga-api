namespace BuildingBlocks.Domain
{
    public abstract class Entity<TEntityId> : IEntity
    {
        private List<IDomainEvent> _domainEvents = [];

        protected Entity(TEntityId id) => Id = id;

        protected Entity()
        {

        }

        public TEntityId Id { get; init; }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents?.AsReadOnly();

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new();

            _domainEvents.Add(domainEvent);
        }
    }
}
