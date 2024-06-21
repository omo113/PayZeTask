namespace PayZe.Shared.Abstractions
{
    public abstract class AggregateRoot : Entity, IHasDomainEvent
    {
        private int Version { get; set; }

        private List<object> DomainEvents { get; } = new List<object>();

        public IReadOnlyList<object> PendingDomainEvents()
        {
            return DomainEvents;
        }
        protected virtual void Raise<T>(T @event) where T : DomainEvent
        {
            @event.UId = UId;
            DomainEvents.Add(@event);
        }
    }
}