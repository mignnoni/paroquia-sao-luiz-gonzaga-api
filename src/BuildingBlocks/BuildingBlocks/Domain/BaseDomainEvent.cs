namespace BuildingBlocks.Domain
{
    public abstract record DomainEvent : IDomainEvent
    {
        protected DomainEvent(Guid id, DateTime occurredOn)
            : this()
        {
            Id = id;
            OccurredOn = occurredOn;
        }

        private DomainEvent()
        {
        }

        public Guid Id { get; private set; }

        public DateTime OccurredOn { get; private set; }
    }
}
